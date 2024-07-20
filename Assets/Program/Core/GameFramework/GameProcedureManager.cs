using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger = Ueels.Core.Debug.Logger;

namespace Ueels.Core.GameFramework
{
    /// <summary>
    /// 用法：
    /// 1.建立多个游戏状态，每个游戏状态只负责管理自己的资源加载
    /// 2.建立多个trigger，用于发起游戏流程跳转的请求
    /// 3.建立游戏流程图，将状态和trigger连接成一个状态机
    /// </summary>
    public partial class GameProcedureManager : MonoSingleton<GameProcedureManager>
    {

        /// <summary>
        /// 扩展流程状态
        /// </summary>
        public enum GameState
        {
            MainMenu,
            // StudyLevel,
            // OpenGamePlayMovie,
            // StandardGamePlay,
            // StandardGamePlay_BossDying,
            // GratituteForPlayLevel,
            // LevelSelectMenu,
            // GameSceneLoading,
            // CatShowUpAnimation,
            // CalScore
        }
        /// <summary>
        /// 扩展流程状态跳转的触发信号
        /// </summary>
        public enum GameStateTransitionTrigger
        {
            // OnGameSceneLoadBootFinished,
            // OnOpenStateGamePlayMovieFinished,
            // OnCatShowUpAnimationFinished,
            // OnGratitudeLevelFinished,
            // TutorialFinished,
            // AdjustBeatOffsetFinished,
            // OnStandardGamePlayFinished,
            // LevelSelectClickDown=8,
            // OnMainMenuClickDown,
            // TryJumpToSelectPanel,
            // TryDeadReplay,
            // TryRestartTutorial,
        }
        private void MakeAllGameStates()//构建所有游戏环节对应的状态,这些状态需要通过连接构成游戏运行的graph
        {
            //MainMenu
            gameStateHashTable.Add(GameState.MainMenu, FSM.NewAnonymousState().OnEnter(()=>
            {
                Logger.PrintHint("主界面已加载");
                //加载对应的资源
                //EntranceManager.Instance.LoadScene(EntranceManager.EEntrance.MainMenu);
            }).OnExit(()=>
            {
            }));

            //以下注释全都是用例，可以删掉
            // gameStateHashTable.Add(GameState.AdjustBeatOffset, FSM.NewAnonymousState().OnEnter(()=>
            // {
            //     EntranceManager.Instance.LoadScene(EntranceManager.EEntrance.Calibration);
            // }).OnExit(()=>
            // {
            // }));
            //
            // //临时占位用
            // gameStateHashTable.Add(GameState.CatShowUpAnimation, FSM.NewAnonymousState().OnEnter(()=>
            // {
            //     _MovieTemp("Cool CatShowUPAnimation =W= ",GameStateTransitionTrigger.OnCatShowUpAnimationFinished);
            //
            // }).OnExit(()=>
            // {
            //
            // }));
            //
            //
            //
            // //StudyLevel,
            // gameStateHashTable.Add(GameState.StudyLevel, FSM.NewAnonymousState().OnEnter(()=>
            // {
            //     GameLog.Log("[Turtorial] Begin Turtorial");
            //     EntranceManager.Instance.LoadScene(EntranceManager.EEntrance.Tuning).completed+=(op)=>
            //     {
            //         NiceCat.Art.ArtSceneManager.Instance.SetLevelImmediate(Art.ArtStyle.Tutorial);
            //     };
            // }));
            //
            // //加载游戏资源，主要是玩家和boss
            // gameStateHashTable.Add(GameState.GameSceneLoading, FSM.NewAnonymousState().OnEnter(()=>
            // {
            //     EntranceManager.Instance.LoadScene(EntranceManager.EEntrance.GameScene1).completed+=(op)=>
            //     { 
            //         NiceCat.Art.ArtSceneManager.Instance.SetLevelImmediate(Art.ArtStyle.Blue);
            //     };
            // }));
            //
            // //OpenGamePlayMovie转场动画
            // gameStateHashTable.Add(GameState.OpenGamePlayMovie, FSM.NewAnonymousState().OnEnter(()=>
            // {
            //     // LoadFirstLevel();
            //     if (EntranceManager.Instance.isMainProcedure)
            //     {
            //         LoadTimeline();
            //     }
            //     // LevelDirector.Instance.Init();
            //     // FirstLevelLoaded = true;
            //     System.GC.Collect();
            //     UIManager.Instance.GetGameObjectByType(UIType.LoadingPanel).SetActive(false);
            //     InputManager.Instance.SetGameplayInputActive(false);
            // }).OnExit(()=>
            // {
            //
            // }));
            //
            //
            //
            // //StandardGamePlay,
            // gameStateHashTable.Add(GameState.StandardGamePlay, FSM.NewAnonymousState().OnEnter(()=>
            // {
            //     Player.Instance.ActiveCat();
            //     UIManager.Instance.ShowPanelByType(UIType.GamePanel);
            //     InputManager.Instance.SetGameplayInputActive(true);
            //     BossBase.Instance.StartChase();
            //     //Boss.GetComponentInChildren<BossBase>().StartChase();
            // }).OnExit(()=>
            // {
            //     SetGamePlayConfig(GamePlayConfigure.EasyGP);//结束后总是重置默认为简单配置
            // }));
            //
            // //StandardGamePlay_BossDying,
            // gameStateHashTable.Add(GameState.StandardGamePlay_BossDying, FSM.NewAnonymousState().OnEnter(()=>
            // {
            //     Player.Instance.ActiveCat();
            //     UIManager.Instance.ShowPanelByType(UIType.GamePanel);
            //     InputManager.Instance.SetGameplayInputActive(true);
            //     BossBase.Instance.StartChase();
            //     StartCoroutine(Timer.DelayDo(1, () =>
            //      {
            //          BossBase.Instance.BossDying();
            //      }));
            // }));
            //
            //
            //
            // //GratituteForPlayLevel,
            // gameStateHashTable.Add(GameState.GratituteForPlayLevel, FSM.NewAnonymousState().OnEnter(()=>
            // {
            //     if(isRoundOneCompleted)
            //     {
            //         ProcessTigger_AndFadeinFadeout(GameStateTransitionTrigger.OnGratitudeLevelFinished);
            //
            //     }
            //     else
            //     {
            //         isRoundOneCompleted = true;
            //         EntranceManager.Instance.LoadScene(EntranceManager.EEntrance.CurtainCall).completed += (op) =>
            //         {
            //             NiceCat.Art.ArtSceneManager.Instance.SetLevelImmediate(Art.ArtStyle.Tutorial);
            //         };
            //
            //     }
            // }).OnExit(()=>
            // {
            // }));
            //
            // gameStateHashTable.Add(GameState.CalScore, FSM.NewAnonymousState().OnEnter(() =>
            // {
            //     UIManager.Instance.ShowPanelByType(UIType.WinPanel);
            //     //FadePanel.Instance.OnBlackOut();
            //
            // }).OnExit(() =>
            // {
            //
            // }));
            //
            // //LevelSlectMenu
            // gameStateHashTable.Add(GameState.LevelSelectMenu, FSM.NewAnonymousState().OnEnter(()=>
            // {
            //     EntranceManager.Instance.LoadScene(EEntrance.MainMenu);
            //     UIManager.Instance.ShowPanelByType(UIType.SelectLevelPanel);
            // UIManager.Instance.GetGameObjectByType(UIType.SelectLevelPanel).GetComponentInChildren<NiceCat.UI.SelectLevelPanel>().SetLevelNameAndHighScore(MaxScore);
            //     UIManager.Instance.GetGameObjectByType(UIType.SelectLevelPanel).GetComponentInChildren<NiceCat.UI.SelectLevelPanel>().OnShow();
            //
            // }));
        }

        #region 流程图建立
        private void BuildMainProcessGraph()//将状态和转换连接成游戏主流程
        {
            gsm.ClearGraph();
            InsertFSMState(gsm, GameState.MainMenu);//主菜单
            // InsertFSMState(gsm, GameState.AdjustBeatOffset);//调音
            // InsertFSMState(gsm, GameState.CatShowUpAnimation);//出场动画
            // InsertFSMState(gsm, GameState.StudyLevel);//教学关卡
            // InsertFSMState(gsm, GameState.GameSceneLoading);//资源加载(程序流程)
            // InsertFSMState(gsm, GameState.OpenGamePlayMovie);//转场动画
            // InsertFSMState(gsm, GameState.StandardGamePlay);//正式游玩
            // InsertFSMState(gsm, GameState.GratituteForPlayLevel);//谢幕关卡
            // InsertFSMState(gsm, GameState.CalScore);//胜利结算界面
            // InsertFSMState(gsm, GameState.LevelSelectMenu);//关卡选择

            //游玩主线
            // InsertFSMTransition(gsm,GameState.MainMenu, GameState.AdjustBeatOffset, GameStateTransitionTrigger.OnMainMenuClickDown);
            // InsertFSMTransition(gsm,GameState.AdjustBeatOffset, GameState.CatShowUpAnimation, GameStateTransitionTrigger.AdjustBeatOffsetFinished);
            // InsertFSMTransition(gsm,GameState.CatShowUpAnimation, GameState.StudyLevel, GameStateTransitionTrigger.OnCatShowUpAnimationFinished);
            // InsertFSMTransition(gsm,GameState.StudyLevel, GameState.GameSceneLoading, GameStateTransitionTrigger.TutorialFinished);
            // InsertFSMTransition(gsm,GameState.GameSceneLoading, GameState.OpenGamePlayMovie, GameStateTransitionTrigger.OnGameSceneLoadBootFinished);
            // InsertFSMTransition(gsm,GameState.OpenGamePlayMovie, GameState.StandardGamePlay, GameStateTransitionTrigger.OnOpenStateGamePlayMovieFinished); ;
            // InsertFSMTransition(gsm,GameState.StandardGamePlay, GameState.GratituteForPlayLevel, GameStateTransitionTrigger.OnStandardGamePlayFinished); 
            // InsertFSMTransition(gsm,GameState.GratituteForPlayLevel, GameState.CalScore, GameStateTransitionTrigger.OnGratitudeLevelFinished);
            //
            // InsertFSMTransition(gsm, GameState.CalScore, GameState.LevelSelectMenu, GameStateTransitionTrigger.TryJumpToSelectPanel);
            //
            // InsertFSMTransition(gsm, GameState.StandardGamePlay, GameState.LevelSelectMenu, GameStateTransitionTrigger.TryJumpToSelectPanel);
            // InsertFSMTransition(gsm, GameState.LevelSelectMenu, GameState.GameSceneLoading, GameStateTransitionTrigger.LevelSelectClickDown);
            //
            // //其他跳转
            // InsertFSMTransition(gsm,GameState.StandardGamePlay, GameState.GameSceneLoading, GameStateTransitionTrigger.TryDeadReplay);
            // InsertFSMTransition(gsm,GameState.StudyLevel, GameState.StudyLevel, GameStateTransitionTrigger.TryRestartTutorial);
        }
        private void BuildTestProcessGraph()
        {
            gsm.ClearGraph();
            // InsertFSMState(gsm, GameState.GameSceneLoading);
            // InsertFSMState(gsm, GameState.StandardGamePlay);//正式游玩
            //
            // InsertFSMTransition(gsm,GameState.GameSceneLoading, GameState.StandardGamePlay, GameStateTransitionTrigger.OnGameSceneLoadBootFinished);
            // InsertFSMTransition(gsm,GameState.StandardGamePlay, GameState.GameSceneLoading, GameStateTransitionTrigger.TryDeadReplay);
            // //InsertFSMTransition(gsm,GameState.StandardGamePlay, GameState.StandardGamePlay, GameStateTransitionTrigger.);
        }

        #endregion



        
        #region 带特殊转场的触发器实现
        /// <summary>
        /// 项目定义自己的环节跳转转场效果（例如淡入淡出等）
        /// </summary>
        public void ProcessTrigger(GameStateTransitionTrigger eventTrigger)//这是一个基本的triger实现
        {
            if(!gsm.Triger((int)eventTrigger))
            {
                Logger.PrintError("[系统消息]游戏流程触发失败，当前状态:"+(GameState)gsm.CurrentStateCode+"/触发器:" + eventTrigger);
            }
            Logger.PrintHint("[系统消息]当前游戏流程:" + (GameState)gsm.CurrentStateCode);
        }
        
        /// <summary>
        /// 特殊trigger，带fade转场
        /// </summary>
        /// <param name="eventTrigger"></param>
        /// <param name="isNewSceneLoadHappened">如果有新scene加载，会在加载scene后打开全屏黑雾，否则会当即打开黑雾</param>
        public void ProcessTigger_FadeinFadeout(GameStateTransitionTrigger eventTrigger,bool isNewSceneLoadHappened=true)
        {
            // FadePanel.Instance.underFadeBlackDo= () =>
            //       {
            //           ProcessTrigger(eventTrigger);
            //           if(!isNewSceneLoadHappened)
            //           {
            //               FadePanel.Instance.OnBlackOut();
            //
            //           }
            //       };
            // FadePanel.Instance.gameObject.SetActive(true);
        }
        #endregion




        //[LabelText("流程入口")]
        public enum EProcedure
        {
            //[LabelText("游戏场景1")]
            MainProcedure,
            GamePlayTest,
        }

        //[EnumPaging]
        public EProcedure entrance;
        private void Start()
        {
            MakeAllGameStates();
            //EntranceManager.Instance.LoadGlobalResource(); //TODO:资源方面的预处理等等
            //EntranceManager.Instance.ReloadGlobalState();
            switch(entrance)
            {
                case EProcedure.MainProcedure:
                    BuildMainProcessGraph();
                    if(!gsm.Enter((int)GameState.MainMenu))
                        Logger.PrintError("入口状态未找到");
                    break;
                // case EProcedure.GamePlayTest:
                //     BuildTestProcessGraph();
                // if(!gsm.Enter((int)GameState.GameSceneLoading))
                //     Logger.PrintError("入口状态未找到");
                //     break;
            }
        }

        #region 游戏流程总控参数
        public static bool isRoundOneCompleted = false;//是否 一周目通关过
        #endregion
    }

    
    public partial class GameProcedureManager
    {
        private static Dictionary<GameState, FSM.StateInfo> gameStateHashTable=new Dictionary<GameState, FSM.StateInfo>();//缓存所有游戏状态
        private FSM gsm = new FSM(); //总游戏流程图
        private  void InsertFSMState( FSM fsm,GameState state)
        {

            if(!gameStateHashTable.ContainsKey(state)||gameStateHashTable[state]==null)
            {
                Logger.PrintError(state+" 对应的函数包为空");
            }
            fsm.AddState((int)state,gameStateHashTable[state]);
        }
        private  void InsertFSMTransition( FSM fsm,GameState from,GameState to,GameStateTransitionTrigger transition)
        {
            fsm.AddTransition((int)from,(int)to,(int)transition);
        }
        public GameState GetCurrentGameState()
        {
            return (GameState)gsm.GetCurrentState();
        }

        public bool IsGameStateEquals(GameState state)
        {
            return GetCurrentGameState() == state;
        }
        
        /// <summary>
        /// 直接跳转到某个游戏环节，慎用
        /// </summary>
        /// <param name="target"></param>
        public void ProcessStateForceSwitch(GameState target)
        {
            gsm.SwitchState((int)target, true);

        }

        
    }
    
}