using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zekzek.JsonContent;

[RequireComponent(typeof(AudioSource))]
public class MusicBehaviour : MonoBehaviour
{
    public static readonly int SAMPLE_FREQUENCY = 44100;
    private const float SPEED_CHANGE = 1.01f;
    private const float MAX_TEMPO_MODIFIER = 2f;
    private static float tempoModifier = 1f;

    public string songID;

    private AudioSource source;
    private float nextNoteTime = 1f;
    private SongData song;

    private IEnumerator<ChordData> chords;

    public static void SpeedUp()
    {
        tempoModifier = Mathf.Min(SPEED_CHANGE * tempoModifier, MAX_TEMPO_MODIFIER);
    }

    public static void SlowDown()
    {
        tempoModifier = Mathf.Max(tempoModifier / SPEED_CHANGE, 1f);
    }

    public static void ResetTempo()
    {
        tempoModifier = 1f;
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
        song = ContentUtil.LoadData<SongData>(songID);
        song.Init();
        chords = song.Chords;
    }

    private void Update()
    {
        if (nextNoteTime <= 0)
        {
            PlayNextNote();
        }
        nextNoteTime -= Time.deltaTime * tempoModifier;
    }

    private void PlayNextNote()
    {
        if (!chords.MoveNext())
        {
            chords.Reset();
            chords.MoveNext();
        }

        float duration = (60f / song.BeatsPerMinute) * chords.Current.Duration / tempoModifier;

        foreach (NoteData note in chords.Current.Notes)
        {
            AudioClip noteClip = GenerateSinNote(note.Frequency, duration, song.NoteFade);
            source.PlayOneShot(noteClip, 1f / chords.Current.Notes.Length);
        }

        nextNoteTime += duration;
    }

    private AudioClip GenerateSinNote(float frequency, float duration, float fade)
    {
        float[] samples = new float[Mathf.RoundToInt(SAMPLE_FREQUENCY * duration)];
        for (int i = 0; i < samples.Length; i++)
        {
            float fadeMult = 1;
            if (i < samples.Length * fade)
                fadeMult = i / (samples.Length * fade);
            else if (i > samples.Length * (1f - fade))
                fadeMult = (samples.Length - i) / (samples.Length * fade);

            samples[i] = Mathf.Sin(Mathf.PI * 2 * i * frequency / SAMPLE_FREQUENCY) * fadeMult;
        }
        AudioClip clip = AudioClip.Create("Note", samples.Length, 1, SAMPLE_FREQUENCY, false);
        clip.SetData(samples, 0);

        return clip;
    }
#if UNITY_EDITOR
    private string songJson;
    public string SongJson
    {
        get { return songJson; }
        set
        {
            songJson = value;
            source = GetComponent<AudioSource>();
            song = JsonUtility.FromJson<SongData>(songJson);
            song.Init();
            chords = song.Chords;
        }
    }
#endif
}