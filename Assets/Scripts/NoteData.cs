
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UtiliZek
{
    [Serializable]
    public class NoteData
    {
        /*****   JSON INTERFACE   *****/
        [SerializeField]
        private string key;

        [SerializeField]
        private int octive;
        /***** END JSON INTERFACE *****/

        public const int SAMPLE_FREQUENCY = 44100;
        public static readonly NoteData REST = new NoteData("REST", 0);
        private static readonly string[] KEYS = new string[] { "C", "D", "E", "F", "G", "A", "B" };
        //https://en.wikipedia.org/wiki/Piano_key_frequencies
        private static readonly float[] FREQUENCIES_AT_OCTIVE_4 = new float[] { 261.6256f, 293.6648f, 329.6276f, 349.2282f, 391.9954f, 440f, 493.8833f };
        private static Dictionary<NoteSearchKey, AudioClip> audioClips = new Dictionary<NoteSearchKey, AudioClip>();

        public float Frequency
        {
            get
            {
                int keyIndex = Array.IndexOf(KEYS, key);
                if (keyIndex < 0)
                    return 0;

                float frequency = FREQUENCIES_AT_OCTIVE_4[keyIndex];
                frequency *= Mathf.Pow(2, octive - 4);
                return frequency;
            }
        }

        public NoteData(string key, int octive)
        {
            this.key = key;
            this.octive = octive;
        }

        public NoteData GenerateOffsetNote(int offset)
        {
            return FromKeyIndex(ToKeyIndex() + offset);
        }

        public NoteData GenerateOffsetOctive(int offset)
        {
            return GenerateOffsetNote(offset * KEYS.Length);
        }

        private int ToKeyIndex()
        {
            int keyIndex = Array.IndexOf(KEYS, key);
            if (keyIndex < 0)
                return 0;
            return octive * KEYS.Length + keyIndex;
        }

        private NoteData FromKeyIndex(int keyIndex)
        {
            return new NoteData(KEYS[keyIndex % KEYS.Length], keyIndex / KEYS.Length);
        }

        public static AudioClip GenerateAdsrSinNote(NoteData noteData, float duration)
        {
            NoteSearchKey searchKey = new NoteSearchKey(noteData, duration);
            if (!audioClips.ContainsKey(searchKey))
            {
                Debug.Log("Generating audio clip for " + searchKey);
                float[] samples = new float[Mathf.RoundToInt(SAMPLE_FREQUENCY * duration)];
                for (int i = 0; i < samples.Length; i++)
                    samples[i] = makeSinSample(i, noteData.Frequency, calcAdsrMagnitude(((float)i) / samples.Length, new Vector2(0.05f, 1f), new Vector2(0.3f, 0.7f), new Vector2(0.8f, 0.7f)));
                AudioClip clip = AudioClip.Create("Note", samples.Length, 1, SAMPLE_FREQUENCY, false);
                clip.SetData(samples, 0);
                audioClips[searchKey] = clip;
            }

            return audioClips[searchKey];
        }

        private static float calcAdsrMagnitude(float percent, params Vector2[] positions)
        {
            //TODO: verify valid numbers?
            Vector2 start = Vector2.zero;
            Vector2 end = Vector2.zero;

            foreach (Vector2 pos in positions)
            {
                start = end;
                end = pos;
                if (end.x >= percent)
                    break;
            }
            if (percent > end.x)
            {
                start = end;
                end = new Vector2(1, 0);
            }

            return Vector2.Lerp(start, end, (percent - start.x) / (end.x - start.x)).y;
        }

        private static float makeSinSample(int index, float frequency, float magnitude)
        {
            return magnitude * Mathf.Sin(2 * Mathf.PI * index * frequency / SAMPLE_FREQUENCY);
        }

        private static float makeSquareSample(int index, float frequency, float magnitude)
        {
            return Mathf.Sin(2 * Mathf.PI * index * frequency / SAMPLE_FREQUENCY) > 0 ? magnitude : -magnitude;
        }

        private static float makeTriangleSample(int index, float frequency, float magnitude)
        {
            return Mathf.Sin(2 * Mathf.PI * index * frequency / SAMPLE_FREQUENCY) > 0 ? magnitude : -magnitude;
        }

        private class NoteSearchKey
        {
            private NoteData noteData;
            private float duration;

            public NoteSearchKey(NoteData noteData, float duration)
            {
                this.noteData = noteData;
                this.duration = duration;
            }

            public override bool Equals(object other)
            {
                if (!(other is NoteSearchKey))
                    return false;

                NoteSearchKey otherKey = (NoteSearchKey)other;

                return noteData.Frequency == otherKey.noteData.Frequency
                        && duration == otherKey.duration;
            }

            public override int GetHashCode()
            {
                return (int)(100000 * noteData.Frequency * duration);
            }
        }
    }
}