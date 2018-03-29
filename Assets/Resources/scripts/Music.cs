using UnityEngine;
using UnityEngine.UI;

class Music {
    private Music() {}

    private static Music instance;

    public static Music GetInstance()
    {
        if (instance == null)
            instance = new Music();
        return instance;
    }

    private float volume;
    private float time;
    private AudioSource audio;
    private GameObject parentObject;
    private static bool playing = true;

    public void PlayGlobalMusic(GameObject newParent)
    {
        if (parentObject == null || parentObject != newParent)
            parentObject = newParent;

        if (audio == null) {
            audio = parentObject.GetComponent<AudioSource>();
            if (audio == null)
                audio = parentObject.AddComponent<AudioSource>();
            audio.clip = Resources.Load<AudioClip>("sounds/music");
            audio.time = Mathf.Max(audio.time, time);
            audio.volume = Mathf.Min(audio.volume, volume);
            audio.loop = false;
        }

        audio.volume = Mathf.Min(Mathf.Min(1.0f, audio.time / 10.0f), (audio.clip.length - audio.time - 3.0f) / 10.0f);
        if (audio.clip.length - audio.time < 3.0f) // Залепа. Цикл начинается со времени, установленного выше при иницилазиазации аудио
            audio.time = 0.0f;
        volume = audio.volume;
        time = audio.time;
        
        if (!audio.isPlaying) {
            audio.Play();
        }   
    }

    public static void Update()
    {
        instance = GetInstance();
        if (playing && (instance.audio == null || instance.audio.isPlaying))
            instance.PlayGlobalMusic(GameObject.FindGameObjectWithTag("MainCamera"));
    }

    public bool SwitchSound(RawImage line)
    {
        Debug.Assert(audio != null);
        if (audio.isPlaying)
            StopGlobalMusic();
        else
            PlayGlobalMusic(null);
        line.enabled = !audio.isPlaying;
        playing = audio.isPlaying;
        return audio.isPlaying;
    }

    public void StopGlobalMusic()
    {
        if (audio != null && audio.isPlaying)
            audio.Stop();
    }
}
