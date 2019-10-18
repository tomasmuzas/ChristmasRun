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

        private readonly EventHandler<CollisionHappenedEvent> _collisionHandler = new EventHandler<CollisionHappenedEvent>();
        private readonly EventHandler<PlayerJumpedEvent> _jumpHandler = new EventHandler<PlayerJumpedEvent>();

        public void Awake()
        {
            backgroundMusicAudioSource = gameObject.AddComponent<AudioSource>();
            actionAudioSource = gameObject.AddComponent<AudioSource>();

            _collisionHandler.EventAction += ev => PlayGameOverSounds();
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
            backgroundMusicAudioSource.Play();
        }

        private void PlayGameOverSounds()
        {
            backgroundMusicAudioSource.Stop();
            backgroundMusicAudioSource.clip = GameOverSound;
            backgroundMusicAudioSource.Play();
        }
    }
}
