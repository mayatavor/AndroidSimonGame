using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace maya_simon
{
    [Service]
    public class MusicService : Service
    {
        public MediaPlayer player;
        static bool destroy = false;
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        //create a music player
        public override void OnCreate()
        {
            base.OnCreate();
            player = MediaPlayer.Create(this, Resource.Raw.maps);
            player.Looping = true;
            player.SetVolume(100, 100);
        }

        //plays the music
        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int StartId)
        {
            CreateNotificationChannel();
            ISharedPreferences sp;
            sp = this.GetSharedPreferences("playbackDetails", FileCreationMode.Private);

            if (sp != null)
            {
                int newPos = sp.GetInt("timeNow", 0);
                player.SeekTo(newPos);
            }
            player.Start();
            destroy = false;
            Intent NotificationIntent = new Intent(this, typeof(MainActivity));
            PendingIntent pendingIntent = PendingIntent.GetActivity(this, requestCode: 0, NotificationIntent, flags: 0);

            Notification notification = new Android.Support.V4.App.NotificationCompat.Builder(context: this, channelId: "ChannelID1")
                    .SetContentTitle(Resources.GetString(Resource.String.app_name))
                    .SetContentText("Media Player Service Text")
                    .SetSmallIcon(Resource.Drawable.icon_small_blue)
                    .SetContentIntent(pendingIntent)
                    .SetOngoing(true)
                    .Build();

            StartForeground(id: 1, notification);
            return StartCommandResult.Sticky;
        }

        private void CreateNotificationChannel()
        {
            NotificationChannel notificationChannel = new NotificationChannel(id: "ChannelID1", name: "Forground Notification",
                                                                importance: NotificationImportance.Default);
            NotificationManager manager = (NotificationManager)GetSystemService(Context.NotificationService);
            manager.CreateNotificationChannel(notificationChannel);
        }

        //saves the time that the music stops
        //in order to keep playing from the same place the next time
        public override void OnDestroy()
        {
            if (destroy)
                return;

            if (player != null)
            {
                ISharedPreferences sp;
                sp = this.GetSharedPreferences("playbackDetails", FileCreationMode.Private);
                Android.Content.ISharedPreferencesEditor editor = sp.Edit();
                editor.PutInt("timeNow", player.CurrentPosition);
                editor.Commit();

                player.Stop();
                player.Release();
            }
            base.OnDestroy();
            StopForeground(true);
                    StopSelf();
            destroy = true;
        }
    }
}