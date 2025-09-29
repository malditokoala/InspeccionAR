using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpeechBubble
{

    /// <summary>
    /// Used for Legacy text
    /// </summary>
    public class SpeechBubble_Legacy : SpeechBubble
    {
        [SerializeField]
        [Tooltip("The text that holds the dialogue")]
        private Text dialogueTextComponent;

        private void Start()
        {
            updateSpeechBubble();
        }

        /// <summary>
        /// Sets the dialogue text to the given string
        /// </summary>
        /// <param name="text">The string to set the dialogue to</param>
        public override void setDialogueText(string text)
        {
            dialogueText = text;
            dialogueTextComponent.text = text;
        }

        /// <summary>
        /// Sets the dialogue text to the color
        /// </summary>
        /// <param name="color"></param>
        public override void setDialogueTextColor(Color color)
        {
            dialogueTextComponent.color = color;
        }

        /// <summary>
        /// Updates the text to match with the values in the inspector
        /// </summary>
        protected override void updateTextGraphics()
        {
            dialogueTextComponent.text = dialogueText;
        }
    }
}
