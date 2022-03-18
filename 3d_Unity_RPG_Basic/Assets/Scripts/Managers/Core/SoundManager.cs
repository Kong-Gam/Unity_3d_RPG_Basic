using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    // Sound�� ������ �迭 �з�
    AudioSource[] _audioSources = new AudioSource[(int)Defines.Sounds.MaxCount];
    // Sound ������ ���� �����̳�
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if(root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Defines.Sounds));

            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.GetOrAddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            _audioSources[(int)Defines.Sounds.Bgm].loop = true;

        }
    }

    public void Clear()
    {
        foreach(AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        _audioClips.Clear();
    }

    public void Play(string path, Defines.Sounds type = Defines.Sounds.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    public void Play(AudioClip audioClip, Defines.Sounds type = Defines.Sounds.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        switch(type)
        {
            case Defines.Sounds.Bgm:
                PlayBgm(audioClip, pitch);
                break;
            case Defines.Sounds.Effect:
                PlayEffect(audioClip, pitch);
                break;
        }
    }

    AudioClip GetOrAddAudioClip(string path, Defines.Sounds type = Defines.Sounds.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if(type == Defines.Sounds.Bgm)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }
        else
        {
            // ���� ���� ������ �ִ� Ÿ���� Clip�� Load �ּ�ȭ
            // �ѹ� ���Ǿ��� Clip�� �����̳ʿ� ��������Ƿ� �����ͼ� ����Ѵ�.
            if(_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing : {path}");

        return audioClip;
    }

    private void PlayBgm(AudioClip audioClip, float pitch = 1.0f)
    {
        AudioSource audioSource = _audioSources[(int)Defines.Sounds.Bgm];

        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.pitch = pitch;
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private void PlayEffect(AudioClip audioClip, float pitch = 1.0f)
    {
        AudioSource audioSource = _audioSources[(int)Defines.Sounds.Effect];
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(audioClip); // ���ϴ� Clip ��ø ���
    }
}
