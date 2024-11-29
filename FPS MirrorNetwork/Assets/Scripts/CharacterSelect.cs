using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

namespace DapperDino.Mirror.Tutorials.CharacterSelection
{
    public class CharacterSelect : NetworkBehaviour
    {
        [SerializeField] private GameObject characterSelectDisplay = default;
        [SerializeField] private Transform characterPreviewParent = default;
        [SerializeField] private TMP_Text characterNameText = default;
        [SerializeField] private float turnSpeed = 90f;
        [SerializeField] private Character[] characters = default;
        [SerializeField] private TMP_Text time = default;

        private int currentCharacterIndex = 0;
        private List<GameObject> characterInstances = new List<GameObject>();

        public override void OnStartClient()
        {
            
            if (characterPreviewParent.childCount == 0)
            {
                foreach (var character in characters)
                {
                    GameObject characterInstance =
                        Instantiate(character.CharacterPreviewPrefab, characterPreviewParent);

                    characterInstance.SetActive(false);

                    characterInstances.Add(characterInstance);
                }
            }

            characterInstances[currentCharacterIndex].SetActive(true);
            characterNameText.text = characters[currentCharacterIndex].CharacterName;

            characterSelectDisplay.SetActive(true);
            StartCoroutine(CoolDown(20));
        }

        private void Update()
        {
            characterPreviewParent.RotateAround(
                characterPreviewParent.position,
                characterPreviewParent.up,
                turnSpeed * Time.deltaTime);
        }

        public void Select()
        {
            Debug.Log("SElect");
            //CmdSelect(currentCharacterIndex);
            CmdDisplayIdConnection();

            //characterSelectDisplay.SetActive(false);
        }

        [Command(requiresAuthority = false)]
        public void CmdSelect(int characterIndex, NetworkConnectionToClient sender = null)
        {
            // Debug.Log("Hello");
            // Debug.Log("connection id:" + sender.connectionId);
            GameObject characterInstance = Instantiate(characters[characterIndex].GameplayCharacterPrefab);
            characterInstance.name = $"{characters[characterIndex].GameplayCharacterPrefab.name} [connId={sender.connectionId}]";
            NetworkServer.Spawn(characterInstance, sender);
            NetworkServer.ReplacePlayerForConnection(sender,characterInstance);
        }

        [Command(requiresAuthority = false)]
        public void CmdDisplayIdConnection(NetworkConnectionToClient sender = null){
            Debug.Log("connection id :" + sender.connectionId);
            Debug.Log("Hello");
        }

        public void Right()
        {
            //Debug.Log("Right");
            characterInstances[currentCharacterIndex].SetActive(false);

            currentCharacterIndex = (currentCharacterIndex + 1) % characterInstances.Count;

            characterInstances[currentCharacterIndex].SetActive(true);
            characterNameText.text = characters[currentCharacterIndex].CharacterName;
        }

        public void Left()
        {
            //Debug.Log("Left");
            characterInstances[currentCharacterIndex].SetActive(false);

            currentCharacterIndex--;
            if (currentCharacterIndex < 0)
            {
                currentCharacterIndex += characterInstances.Count;
            }

            characterInstances[currentCharacterIndex].SetActive(true);
            characterNameText.text = characters[currentCharacterIndex].CharacterName;
        }

        private IEnumerator CoolDown(int numToCount){
            int i = numToCount;
            while(i >= 0){
                time.text = "Time :" + i;
                yield return new WaitForSecondsRealtime(1);
                i -= 1;
            }
            CmdSelect(currentCharacterIndex);
            characterSelectDisplay.SetActive(false);
        }
        
    }
}
