using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Gem.Sound
{
   
    /// <summary>
    /// Plays SoundEffects and Songs using the flyweight pattern
    /// </summary>
    public class SoundManager
    {

        #region Fields

        private Dictionary<string, SoundEffect> sfx;
        private Dictionary<string, Song> songs;

        #endregion


        #region Constructor

        public SoundManager()
        {
            sfx = new Dictionary<string, SoundEffect>();
            songs = new Dictionary<string, Song>();
        }

        #endregion


        #region Add / Remove

        /// <summary>
        /// Adds a new sound effect in a dictionary by the specified tag.
        /// Supported formats: wav
        /// </summary>
        /// <param name="sfxTag">The tag</param>
        /// <param name="soundfx">The soundeffect</param>
        public void AddSfx(string sfxTag, SoundEffect soundfx)
        {
            try
            {
                sfx.Add(sfxTag, soundfx);
            }
            catch (Exception ex)
            {
                // LogHelper.Error("Failed to load sfx {0} : {1}", soundfx.Name, ex.Message);
            }
        }


        /// <summary>
        /// Removes the soundeffect from the sfx dictionary by the tag
        /// </summary>
        /// <param name="sfxTag">The sfx tag</param>
        public void RemoveSfx(string sfxTag)
        {
            if (sfx.ContainsKey(sfxTag))
            {
                sfx.Remove(sfxTag);
            }
        }


        /// <summary>
        ///  Adds a new song in a dictionary by the specified tag.
        ///  Supported formats: mp3
        /// </summary>
        /// <param name="songTag">The tag</param>
        /// <param name="song">The song</param>
        public void AddSong(string songTag, Song song)
        {
            try
            {
                songs.Add(songTag, song);

            }
            catch (Exception ex)
            {
                //LogHelper.Error("Failed to load song {0} : {1}", song.Name, ex.Message);
            }
        }


        /// <summary>
        /// Removes the song from the song dictionary by the tag
        /// </summary>
        /// <param name="sfxTag">The song tag</param>
        public void RemoveSong(string songTag)
        {
            if (songs.ContainsKey(songTag))
            {
                songs.Remove(songTag);
            }
        }

        #endregion


        #region Play / Stop


        /// <summary>
        /// Looks if the specified soundEffect is loaded and plays it
        /// </summary>
        /// <param name="sfxTag">The tag</param>
        /// <param name="volume">The volume. Default 1.0f = full </param>
        /// <param name="pitch">The pitch. Default 0.0f</param>
        /// <param name="pan">The pan. Default 0.0f</param>
        public void PlaySFX(string sfxTag, float volume = 1.0f, float pitch = 0.0f, float pan = 0.0f)
        {
            try
            {
                // if (sfx.ContainsKey(sfxName))
                sfx[sfxTag].Play(volume, pitch, 0.0f);
            }
            catch (Exception ex)
            {
                //LogHelper.Error("Failed to play sfx with tag {0} : {1}", sfxTag, ex.Message);
            }
        }


        /// <summary>
        /// Looks if the specified song is loaded and plays it
        /// </summary>
        /// <param name="songTag">The songs tag</param>
        /// <param name="volume">The songs volume. Default full = 1.0f </param>
        /// <param name="repeat">If the song is on repeat. Default false</param>
        public void PlaySong(string songTag, float volume = 1.0f, bool repeat = false)
        {
            try
            {
                MediaPlayer.IsRepeating = repeat;
                MediaPlayer.Volume = volume;
                MediaPlayer.Play(songs[songTag]);
            }
            catch (Exception ex)
            {
                //LogHelper.Error("Failed to play song with tag {0} : {1}", songTag, ex.Message);
            }
        }


        /// <summary>
        /// Stops the current song
        /// </summary>
        public void Stop()
        {
            MediaPlayer.Stop();
        }
        
        #endregion


        #region Sound Helpers

        /// <summary>
        /// Sets the Media Player's volume
        /// </summary>
        public void SetVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }


        #endregion

    }
}