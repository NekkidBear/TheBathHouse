using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DynamicSpriteSheetAnimator : MonoBehaviour
{
    public string baseFolderPath = "Assets/sprites/characters/male/";
    public string[] bodyStyles = { "1_buff", "2_athletic", "3_bulky", "4_big" }; // Body styles
    public string[] skinTones = { "a", "b", "c", "d", "e" }; // Skin tones
    public int spriteWidth = 72;
    public int spriteHeight = 72;
    public string[] directions = { "Down", "Left", "Right", "Up" };

    private Dictionary<string, AnimationClip> animationClips = new Dictionary<string, AnimationClip>();

    void Start()
    {
        GenerateAnimations();
    }

    void GenerateAnimations()
    {
        foreach (string bodyStyle in bodyStyles)
        {
            foreach (string skinTone in skinTones)
            {
                string filePath = $"{baseFolderPath}{bodyStyle}/body/{bodyStyle}_{skinTone}.png";
                Texture2D spriteSheet = AssetDatabase.LoadAssetAtPath<Texture2D>(filePath);
                if (spriteSheet == null) continue;

                Sprite[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(filePath) as Sprite[];
                if (sprites == null) continue;

                for (int faceIndex = 0; faceIndex < 8; faceIndex++)
                {
                    for (int dirIndex = 0; dirIndex < directions.Length; dirIndex++)
                    {
                        List<Sprite> animationFrames = new List<Sprite>();

                        for (int frameIndex = 0; frameIndex < 3; frameIndex++)
                        {
                            int spriteIndex = (faceIndex * 4 * 12) + (dirIndex * 12) + frameIndex;
                            animationFrames.Add(sprites[spriteIndex]);
                        }

                        string animName = $"{bodyStyle}_{skinTone}_face{faceIndex + 1}_{directions[dirIndex]}";
                        CreateAnimationClip(animationFrames.ToArray(), animName);
                    }
                }
            }
        }
    }

    void CreateAnimationClip(Sprite[] frames, string animName)
    {
        AnimationClip clip = new AnimationClip
        {
            frameRate = 12,
            name = animName
        };

        EditorCurveBinding spriteBinding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };

        ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[frames.Length];

        for (int i = 0; i < frames.Length; i++)
        {
            keyFrames[i] = new ObjectReferenceKeyframe
            {
                time = i / clip.frameRate,
                value = frames[i]
            };
        }

        AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, keyFrames);
        string path = $"Assets/Animations/{animName}.anim";
        AssetDatabase.CreateAsset(clip, path);
        AssetDatabase.SaveAssets();

        animationClips[animName] = clip;
    }

    public AnimationClip GetAnimationClip(string bodyStyle, string skinTone, int faceIndex, string direction)
    {
        string animName = $"{bodyStyle}_{skinTone}_face{faceIndex + 1}_{direction}";
        return animationClips.TryGetValue(animName, out AnimationClip clip) ? clip : null;
    }
}
