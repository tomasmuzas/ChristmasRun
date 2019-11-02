using Assets.Scripts.EventHandling;
using Assets.Scripts.Events;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class SoundManager: MonoBehaviour
    {
        public AudioClip BackgroundMusic;
        public AudioClip JumpSound;
        public AudioClip GameOverSound;
        private AudioSource backgroundMusicAudioSource;
        private AudioSource actionAudioSource;

        private readonly EventHandler<GameOverEvent> _gameOverHandler = new EventHandler<GameOverEvent>();
        private readonly EventHandler<PlayerJumpedEvent> _jumpHandler = new EventHandler<PlayerJumpedEvent>();

        public void Start()
        {
            backgroundMusicAudioSource = gameObject.AddComponent<AudioSource>();
            actionAudioSource = gameObject.AddComponent<AudioSource>();

            _gameOverHandler.EventAction += ev => PlayGameOverSounds();
            _jumpHandler.EventAction += ev => PlayJumpSound();
            StartBackgroundMusic();
        }

        private void PlayJumpSound()
        {
            actionAudioSource.clip = JumpSound;
            actionAudioSource.Play();
        }

        private void StartBackgroundMusic()
        {
            backgroundMusicAudioSource.clip = BackgroundMusic;
            backgroundMusicAudioSource.loop = true;
            backgroundMusicAudioSource.Play();
        }

        private void PlayGameOverSounds()
        {
            backgroundMusicAudioSource.loop = false;
            backgroundMusicAudioSource.Stop();
            backgroundMusicAudioSource.clip = GameOverSound;
            backgroundMusicAudioSource.Play();
        }
    }
}
