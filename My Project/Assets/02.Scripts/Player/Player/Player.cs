using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 유령 캐릭터를 저장할 변수
    public GameObject playerGhost;

    // 쉴드 캐릭터를 저장할 변수
    public GameObject playerShield;

    // 고글 캐릭터를 저장할 변수
    public GameObject playerGoggles;

    // 청소부 캐릭터를 저장할 변수
    public GameObject playerCleaner;

    // 스크립트 캐시처리를 할 변수
    private PlayerGhostController playerGhostControllerScr;
    private PlayerShieldController playerShieldControllerScr;
    private PlayerGogglesController playerGogglesControllerScr;
    private PlayerCleanerController playerCleanerControllerScr;

    // 플레이어 빙의 상태를 표현할 변수
    private bool isPossesing;
    // 현재 플레이어 캐릭터 상태를 저장할 변수
    private PlayerType currentPlayer;

    // Start is called before the first frame update
    void Awake()
    {

        // 캐쉬 설정
        CashScript();
        // 스크립트 설정
        Init();


        isPossesing = false;
        // 시작 타입을 유령 타입으로 설정
        currentPlayer = PlayerType.PLAYERGHOST;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool IsPossesing
    {
        get
        {
            return isPossesing;
        }
        set
        {
            this.isPossesing = value;
        }
    }

    // 스크립트 캐시 처리
    public void CashScript()
    {
        playerGhostControllerScr = GetComponent <PlayerGhostController>();
        playerShieldControllerScr = GetComponent <PlayerShieldController>();
        playerGogglesControllerScr = GetComponent <PlayerGogglesController>();
        playerCleanerControllerScr = GetComponent <PlayerCleanerController>();
    }

    public void Init()
    {
        // 유령 타입 스크립트만 활성화하고 나머지는 비활성화.
        playerGhostControllerScr.enabled = true;
        playerShieldControllerScr.enabled = false;
        playerGogglesControllerScr.enabled = false;
        playerCleanerControllerScr.enabled = false;
    }
    // 플레이어 변경
    public void ChangePlayer(PlayerType player) // 인자로 바꿀 플레이어 타입을 받아온다.
    {
        // 현재 플레이어 캐릭터 오브젝트 비활성화
        inactiveCurrentGameObject(currentPlayer);
        switch (player)
        {
            // 바꿀 캐릭터가 유령캐릭터인 경우
            case PlayerType.PLAYERGHOST:
                // 현재 플레이어 타입을 유령 캐릭터 타입으로 변경 후 유령 게임오브젝트와 스크립트를 활성화
                currentPlayer = PlayerType.PLAYERGHOST;
                playerGhost.SetActive(true);
                playerGhostControllerScr.enabled = true;
                playerGhostControllerScr.SetCashComponent();
                playerGhostControllerScr.ChangePlayerToGhost();
                break;
            // 바꿀 캐릭터가 쉴드캐릭터인 경우
            case PlayerType.PLAYERSHIELD:
                // 현재 플레이어 타입을 쉴드 캐릭터 타입으로 변경 후 쉴드 게임오브젝트와 스크립트를 활성화
                currentPlayer = PlayerType.PLAYERSHIELD;
                playerShield.SetActive(true);
                playerShieldControllerScr.enabled = true;
                playerShieldControllerScr.SetCashComponent();
                break;            
            // 바꿀 캐릭터가 고글 캐릭터인 경우
            case PlayerType.PLAYERGOGGLES:
                // 현재 플레이어 타입을 고글 캐릭터 타입으로 변경 후 고글 게임오브젝트와 스크립트를 활성화
                currentPlayer = PlayerType.PLAYERGOGGLES;
                playerGoggles.SetActive(true);
                playerGogglesControllerScr.enabled = true;
                playerGogglesControllerScr.SetCashComponent();
                break;            
            // 바꿀 캐릭터가 청소부 캐릭터인 경우
            case PlayerType.PLAYERCLEANER:
                // 현재 플레이어 타입을 청소부 캐릭터 타입으로 변경 후 청소부 게임오브젝트와 스크립트를 활성화
                currentPlayer = PlayerType.PLAYERCLEANER;
                playerCleaner.SetActive(true);
                playerCleanerControllerScr.enabled = true;
                playerCleanerControllerScr.SetCashComponent();
                break;

        }
    }
    // 현재 플레이어 캐릭터 비활성화 하는 함수
    private void inactiveCurrentGameObject(PlayerType currPlayer) // 인자로 현재 플레이어 타입을 받아온다.
    {
        switch (currPlayer)
        {
            // 현재 캐릭터가 유령 캐릭터인 경우
            case PlayerType.PLAYERGHOST:
                // 유령 캐릭터 게임 오브젝트와 스크립트를 바활성화
                playerGhost.SetActive(false);
                playerGhostControllerScr.enabled = false;
                break;
            // 현재 캐릭터가 쉴드 캐릭터인 경우
            case PlayerType.PLAYERSHIELD:
                // 쉴드 캐릭터 게임 오브젝트와 스크립트를 바활성화
                playerShield.SetActive(false);
                playerShieldControllerScr.enabled = false;
                break;
            // 현재 캐릭터가 고글 캐릭터인 경우
            case PlayerType.PLAYERGOGGLES:
                // 고글 캐릭터 게임 오브젝트와 스크립트를 바활성화
                playerGoggles.SetActive(false);
                playerGogglesControllerScr.enabled = false;
                break;
            // 현재 캐릭터가 청소부 캐릭터인 경우
            case PlayerType.PLAYERCLEANER:
                // 청소부 캐릭터 게임 오브젝트와 스크립트를 바활성화
                playerCleaner.SetActive(false);
                playerCleanerControllerScr.enabled = false;
                break;

        }
    }

}
