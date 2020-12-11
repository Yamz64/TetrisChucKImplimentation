using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuckAudioHandler : MonoBehaviour
{
    public long[] next_instrument_sequence;
    public long[] next_midi_sequence;
    Chuck.IntArrayCallback intarraycallback;
    long[] last_val;
    ulong size;

    IEnumerator ClearSequence()
    {
        yield return new WaitForSeconds(.5f);
        next_instrument_sequence = new long[0];
        next_midi_sequence = new long[0];
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ChuckSubInstance>().RunCode(@"

            //load all soundfiles in sfx folder into array for easy access

            string sample_paths[20];
            ""C:/Users/LillicrapJp/Documents/GitHub/TetrisChucKImplimentation/MusicTechFinal/Assets/SFX"" => string base;
            base + ""/augmented.wav"" => sample_paths[0];
            base + ""/augmented1.wav"" => sample_paths[1];
            base + ""/augmented2.wav"" => sample_paths[2];
            base + ""/augmented3.wav"" => sample_paths[3];
            base + ""/bassdrum.wav"" => sample_paths[4];
            base + ""/block.wav"" => sample_paths[5];
            base + ""/hat.wav"" => sample_paths[6];
            base + ""/lead.wav"" => sample_paths[7];
            base + ""/leadd1.wav"" => sample_paths[8];
            base + ""/leadd2.wav"" => sample_paths[9];
            base + ""/leadd3.wav"" => sample_paths[10];
            base + ""/majortriad.wav"" => sample_paths[11];
            base + ""/majortriadd1.wav"" => sample_paths[12];
            base + ""/majortriadd2.wav"" => sample_paths[13];
            base + ""/majortriadd3.wav"" => sample_paths[14];
            base + ""/minortriad.wav"" => sample_paths[15];
            base + ""/minortriad1.wav"" => sample_paths[16];
            base + ""/minortriad2.wav"" => sample_paths[17];
            base + ""/minortriad3.wav"" => sample_paths[18];
            base + ""/triangle.wav"" => sample_paths[19];
            
            //load all soundfiles to their buffers    
            
            SndBuf buffers[20];

            for(0 => int i; i < 20; i++){
            	sample_paths[i] => buffers[i].read;
            }
            
            //load the appropriate buffers to the dac

            [0] @=> global int instruments[];
            [0] @=> global int midi_score[];
            global Event play;
            
            while(true){
                play => now;
                for(0 => int i; i<instruments.size(); i++){
                    buffers[instruments[i]] => dac;
                    buffers[instruments[i]].samples() => buffers[instruments[i]].pos;
                    0 => buffers[instruments[i]].pos;
                    buffers[instruments[i]].rate(Std.mtof(midi_score[i])/50);
                }
                500::ms => now;
                for(0 => int i; i<instruments.size(); i++){
                    buffers[instruments[i]] =< dac;
                }
            }
        ");

        intarraycallback = GetComponent<ChuckSubInstance>().CreateGetIntArrayCallback(IntArrayCallBackFunction);
    }

    private void Update()
    {
        GetComponent<ChuckSubInstance>().SetIntArray("instruments", next_instrument_sequence);
        GetComponent<ChuckSubInstance>().GetIntArray("instruments", intarraycallback);
        GetComponent<ChuckSubInstance>().SetIntArray("midi_score", next_midi_sequence);
        GetComponent<ChuckSubInstance>().GetIntArray("midi_score", intarraycallback);

    }

    public void PlayAudio(long[] instrument_sequence, long[] midi_sequence)
    {
        next_instrument_sequence = instrument_sequence;
        next_midi_sequence = midi_sequence;
        GetComponent<ChuckSubInstance>().BroadcastEvent("play");
    }

    void IntArrayCallBackFunction(long[] new_val, ulong length)
    {
    }
}
