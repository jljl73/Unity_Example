using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using Cysharp.Threading.Tasks;

namespace UK.Util
{
    public class CustomAnimator : MonoBehaviour
    {
        struct CustomAnimation
        {
            public List<Sprite>     keyFrames;
        }

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private Dictionary<string, CustomAnimation> animations = new Dictionary<string, CustomAnimation>();
        private CancellationTokenSource cancellationToken;
        private string prevAnimationKey = string.Empty;

        public void ClearAnimation()
        {
            prevAnimationKey = string.Empty;
            if (cancellationToken != null)
                cancellationToken.Cancel();
            animations.Clear();
        }

        ///
        public void LoadAnimation(string inKey, List<Sprite> inKeyFrames)
        {
            animations.Add(inKey, new CustomAnimation() { keyFrames = inKeyFrames });
        }

        public void Play(string inAnimationName)
        {
            if (prevAnimationKey.Equals(inAnimationName))
                return;
            
            prevAnimationKey = inAnimationName;
            if (cancellationToken != null)
                cancellationToken.Cancel();
            cancellationToken = new CancellationTokenSource();

            PlayAnimation(inAnimationName).Forget();
        }

        public async UniTask Play(string inAnimationName, float delay)
        {
            if (prevAnimationKey.Equals(inAnimationName))
                return;
            await UniTask.WaitForSeconds(delay);

            prevAnimationKey = inAnimationName;
            if (cancellationToken != null)
                cancellationToken.Cancel();
            cancellationToken = new CancellationTokenSource();

            PlayAnimation(inAnimationName).Forget();
        }

        async private UniTask PlayAnimation(string inKey)
        {
            int frame = 0;

            do
            {
                var customAni = animations[inKey];
                spriteRenderer.sprite = customAni.keyFrames[frame++];

                await UniTask.Delay(300, cancellationToken: cancellationToken.Token);

                if (frame >= animations[inKey].keyFrames.Count)
                    break;

                //    if (frame >= animations[inKey].keyFrames.Count)
                //        frame = 0;

                //// 단일 프레임일때 루프문 탈출
            } while (animations[inKey].keyFrames.Count > 1);
        }
    }
}