using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public IEnumerator MusicSequence()
    {
        GetComponent<ChuckSubInstance>().RunCode(@"
        //function for playing notes
        fun void PlayNote(int midi_num, float duration){
            PulseOsc osc;
            if(midi_num != -1){
                osc => dac;
                osc.freq(Std.mtof(midi_num));
                osc.gain(.5);        
            }
            duration::ms => now;
            if(midi_num != -1)
            osc =< dac;
        }

        fun void PlayNote2(int midi_num, float duration){
            TriOsc osc;
            if(midi_num != -1){
                osc => dac;
                osc.freq(Std.mtof(midi_num));
                osc.gain(.5);
            }
            duration::ms => now;
            if(midi_num != -1)
            osc =< dac;
        }
        fun void PlayTreble(){
            60.0/170.0 * 1000 => float beat_dur;

            //correspond to the midi number of notes in the song
            [76, 71, 72, 74, 76, 72, 71, 
            69, 69, 72, 76, 74, 72,
            71, 71, 72, 74, 76,
            72, 69, 69,
            -1, 74, 77, 81, 79, 77,
            -1, 76, 72, 76, 74, 72,
            71, 71, 72, 74, 76,
            72, 69, 69,
            76, 71, 72, 74, 76, 72, 71, 
            69, 69, 72, 76, 74, 72,
            71, 71, 72, 74, 76,
            72, 69, 69,
            -1, 74, 77, 81, 79, 77,
            -1, 76, 72, 76, 74, 72,
            71, 71, 72, 74, 76,
            72, 69, 69,
            76, 71, 72, 74, 76,
            72, 69, 72, 76, 81,
            80, 77, 76, 74, 72,
            76, 75, 74, 73,
            72, 69, 71, 72, 69,
            71, 64, 68, 71, 68,
            69, 67, 65, 62,
            69, 67, 65, 62,
            64, 63, 64, 65, 68, 69, 71, 72,
            74, 72, 74, 72, 71, 69, 68, 64,
            68, 69, 71, 72, 74, 75, 76, 80,
            88] @=> int treble_clef[];

            [beat_dur, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur/2, beat_dur/2,
            beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur,
            beat_dur, beat_dur, beat_dur * 2,
            beat_dur/2, beat_dur, beat_dur/2, beat_dur, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur, beat_dur/2, beat_dur, beat_dur/2, beat_dur/2,
            beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur,
            beat_dur, beat_dur, beat_dur * 2,
            beat_dur, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur/2, beat_dur/2,
            beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur,
            beat_dur, beat_dur, beat_dur * 2,
            beat_dur/2, beat_dur, beat_dur/2, beat_dur, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur, beat_dur/2, beat_dur, beat_dur/2, beat_dur/2,
            beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur,
            beat_dur, beat_dur, beat_dur * 2,
            beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur,
            beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur,
            beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur,
            beat_dur, beat_dur, beat_dur, beat_dur,
            beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur,
            beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur,
            beat_dur, beat_dur, beat_dur, beat_dur,
            beat_dur, beat_dur, beat_dur, beat_dur,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur * 2] @=> float treble_durations[];

            for(0 => int i; i<treble_clef.size(); i++){
                PlayNote(treble_clef[i], treble_durations[i]);
            }
        }

        fun void PlayBass(){
            60.0/170.0 * 1000 => float beat_dur;

            //correspond to the midi number of notes in the song
            [47, 52, 50, 52, 28, 52, 47, 52,
            45, 52, 48, 52, 28, 52, 48, 52,
            47, 52, 50, 52, 28, 52, 47, 52,
            45, 52, 48, 52, 45, 52, 48, 52,
            47, 52, 50, 52, 28, 52, 47, 52,
            45, 52, 48, 52, 28, 52, 48, 52,
            47, 52, 50, 52, 28, 52, 47, 52,
            45, 52, 48, 52, 45, 52, 48, 52,
            47, 52, 50, 52, 28, 52, 47, 52,
            45, 52, 48, 52, 28, 52, 48, 52,
            47, 52, 50, 52, 28, 52, 47, 52,
            45, 52, 48, 52, 45, 52, 48, 52,
            47, 52, 50, 52, 28, 52, 47, 52,
            45, 52, 48, 52, 28, 52, 48, 52,
            47, 52, 50, 52, 28, 52, 47, 52,
            45, 52, 48, 52, 45, 52, 48, 52,
            40, 45, 45, 28, 45, 45,
            45, 48, 48, 45, 48, 48,
            40, 44, 44, 40, 44, 44,
            48, 52, 47, 52, 46, 52, 45, 52,
            41, 45, 45, 41, 44, 44,
            40, 44, 44, 40, 44, 44,
            38, 41, 41, 38, 41, 41,
            35, 38, 38, 35, 38, 38,
            28,
            28,
            40, 41, 43, 44, 47, 48, 52, 56, 
            76] @=> int bass_clef[];

            [beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur/2,
            beat_dur/2, beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur/2,
            beat_dur/2, beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur/2,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur/2, beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur/2,
            beat_dur/2, beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur/2,
            beat_dur/2, beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur/2,
            beat_dur/2, beat_dur, beat_dur/2, beat_dur/2, beat_dur, beat_dur/2,
            beat_dur * 4,
            beat_dur * 4,
            beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2, beat_dur/2,
            beat_dur * 2] @=> float bass_durations[];

            for(0 => int i; i<bass_clef.size(); i++){
            PlayNote2(bass_clef[i], bass_durations[i]);
            }
        }

        spork ~PlayTreble();
        spork ~PlayBass();

        40::second => now;");
        yield return new WaitForSeconds(40);
        StartCoroutine(MusicSequence());
    }

    private void Start()
    {
        StartCoroutine(MusicSequence());
    }
}
