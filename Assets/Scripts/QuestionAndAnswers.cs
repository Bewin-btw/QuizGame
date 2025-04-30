using UnityEngine;

[System.Serializable]
public class QuestionAndAnswers
{
    public int level;
    public string Question;
    public string[] Answers;
    public int CorrectAnswer;
    public Sprite questionImage; 
    public AudioClip questionAudio; 
}