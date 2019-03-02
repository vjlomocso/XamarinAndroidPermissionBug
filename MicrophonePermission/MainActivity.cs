using Android.App;
using Android.Widget;
using Android.OS;
using Genetics.Attributes;
using Genetics;
using System;
using Android.Content.PM;
using Android.Runtime;
using Android;

namespace MicrophonePermission
{
    [Activity(Label = "MicrophonePermission", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private const int RecordAudioRequestCode = 0;

        [Splice(Resource.Id.record)]
        private Button _record;
        private Recorder _recorder;

        [SpliceClick(Resource.Id.record)]
        private void RecordClicked(object sender, EventArgs e)
        {
            if (IsRecordAudioGranted())
            {
                ToggleRecord();
            }
            else
            {
                RequestPermissions(new string[] { Manifest.Permission.RecordAudio }, RecordAudioRequestCode);
            }
        }

        private bool IsRecordAudioGranted()
        {
            return CheckSelfPermission(Manifest.Permission.RecordAudio)
                == Permission.Granted;
        }

        private void ToggleRecord()
        {
            if (_recorder.IsRecording)
            {
                _recorder.Stop();
                _record.Text = Resources.GetString(Resource.String.start_record);
            }
            else
            {
                _recorder.Start();
                _record.Text = Resources.GetString(Resource.String.stop_record);
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _recorder = new Recorder();

            Geneticist.Splice(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            switch (requestCode)
            {
                case RecordAudioRequestCode:
                    if (grantResults.Length > 0
                        && grantResults[0] == Permission.Granted)
                    {
                        ToggleRecord();
                    }
                    else
                    {
                        var alertBuilder = new AlertDialog.Builder(this);
                        alertBuilder.SetPositiveButton(Android.Resource.String.Ok,(sender, e) => { });
                        alertBuilder.SetMessage(Resource.String.please_accept_message);
                        var dialog = alertBuilder.Create();
                        dialog.Show();
                    }
                    return;
            }

        }

        protected override void OnDestroy()
        {
            Geneticist.Sever(this);
            _recorder.Dispose();
            base.OnDestroy();
        }
    }
}

