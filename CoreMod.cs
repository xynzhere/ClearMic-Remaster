using GorillaTag.Audio;
using HarmonyLib;
using Photon.Pun;
using Photon.Voice;
using Photon.Voice.Unity;
using Photon.Voice.Unity.UtilityScripts;
using POpusCodec.Enums;
using Seralyth.Managers;
using UnityEngine;

namespace ClearMic
{
    [HarmonyPatch(typeof(Recorder), nameof(Recorder.StartRecording))]
    public class RecorderQualityPatch
    {
        static void Postfix(Recorder __instance)
        {
            if (!PhotonNetwork.InRoom) return;
            if (__instance == null) return;

            __instance.Bitrate = 96000;
            __instance.SamplingRate = SamplingRate.Sampling48000;
            __instance.FrameDuration = OpusCodec.FrameDuration.Frame10ms;
            __instance.ReliableMode = true;
            __instance.VoiceDetection = false;
            __instance.VoiceDetectionThreshold = 0.0f;

            // VoiceManager handles the actual audio pipeline, so gain goes here
            VoiceManager vm = VoiceManager.Get();
            if (vm != null)
                vm.Gain = 20f;

            // WebRTC strip
            WebRtcAudioDsp dsp = __instance.gameObject.GetComponent<WebRtcAudioDsp>();
            if (dsp != null)
            {
                dsp.NoiseSuppression = false;
                dsp.HighPass = false;
                dsp.AEC = false;
                dsp.enabled = false;
            }

            Debug.Log("clearmic remaster quality patch is loaded");
        }
    }

    public class ModEntry
    {
        public static void Init()
        {
            new Harmony("com.xynz.clearmicremaster.patch").PatchAll();
            Debug.Log("clearmic remaster harmony patch is loaded");
        }
    }
}