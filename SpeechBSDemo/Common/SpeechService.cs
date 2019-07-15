/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：SpeechBSDemo.Common
*文件名： SpeechService
*创建人： Lxsh
*创建时间：2019/7/15 11:16:04
*描述
*=======================================================================
*修改标记
*修改时间：2019/7/15 11:16:04
*修改人：Lxsh
*描述：
************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Web;

namespace SpeechBSDemo
{
    public class SpeechService
    {
        private static SpeechSynthesizer synth = null;
        /// <summary>
        /// 返回一个SpeechSynthesizer对象
        /// </summary>
        /// <returns></returns>
        private static SpeechSynthesizer GetSpeechSynthesizerInstance()
        {
            if (synth == null)
            {
                synth = new SpeechSynthesizer();
            }
            return synth;
        }
        /// <summary>
        /// 保存语音文件
        /// </summary>
        /// <param name="text"></param>
        public static void SaveMp3(string strFileName,string spText)
        {    
            synth = GetSpeechSynthesizerInstance();
            synth.Rate = 1;
            synth.Volume = 100;      
            synth.SetOutputToWaveFile(strFileName);
            synth.Speak(spText);
            synth.SetOutputToNull();
        }

    }
}