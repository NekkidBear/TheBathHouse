using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public DynamicSpriteSheetAnimator spriteSheetAnimator;
    public SpriteRenderer spriteRenderer;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetCharacterAnimation(string bodyStyle, string skinTone, int faceIndex)
    {
        animator.runtimeAnimatorController = CreateAnimatorController(bodyStyle, skinTone, faceIndex);
    }

    private RuntimeAnimatorController CreateAnimatorController(string bodyStyle, string skinTone, int faceIndex)
    {
        // Ensure the base animator controller exists in the Resources folder and is of type RuntimeAnimatorController
        var baseAnimatorController = Resources.Load<RuntimeAnimatorController>("BaseAnimatorController");
        if (baseAnimatorController == null)
        {
            Debug.LogError("BaseAnimatorController not found in Resources.");
            return null;
        }

        AnimatorOverrideController overrideController = new AnimatorOverrideController
        {
            runtimeAnimatorController = baseAnimatorController
        };

        foreach (var direction in spriteSheetAnimator.directions)
        {
            string stateName = $"Character_{direction}";
            AnimationClip animClip = spriteSheetAnimator.GetAnimationClip(bodyStyle, skinTone, faceIndex, direction);
            if (animClip != null)
            {
                overrideController[stateName] = animClip;
            }
            else
            {
                Debug.LogWarning($"Animation clip not found for {stateName}.");
            }
        }

        return overrideController;
    }
}
