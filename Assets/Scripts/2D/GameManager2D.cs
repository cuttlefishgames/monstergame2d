using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Monster.Utils;
using DG.Tweening;
using System.Linq;
using System;

public class GameManager2D : Singleton<GameManager2D>
{
    protected override void Awake()
    {
        base.Awake();

        SceneManager.LoadScene("Battlefield 2D", LoadSceneMode.Additive);
    }

    [Header("Test field")]
    public List<AIController2D> LeftSideTeam;
    public List<AIController2D> RightSideTeam;    
    public bool SummonBattleFieldAtStart;

    private void Start()
    {
        if (SummonBattleFieldAtStart)
        {
            TestBattlefied();
        }
    }

    public void TestBattlefied()
    {
        if (Battlefield2D.IsShowing)
        {
            return;
        }

        //var allAicontrollers = FindObjectsOfType<AIController2D>();
        //foreach (var aiC in allAicontrollers)
        //{
        //    aiC.Agent.isStopped = true;
        //}

        LeftSideTeam.Concat(RightSideTeam).ToList().ForEach(e => e.Idle());
        LeftSideTeam.Concat(RightSideTeam).ToList().ForEach(e => e.SetAgentState(false));

        transform.DOMove(transform.position, 1.2f).OnComplete(() => 
        {
            CorretDirection(LeftSideTeam.Concat(RightSideTeam).ToList(), Battlefield2D.GetPositionByTag(BattlefieldPositions.MIDDLE));
            LeftSideTeam.Concat(RightSideTeam).ToList().ForEach(e => e.SetSpritesObjectsLayer(Battlefield2D.LAYER_NAME));
        });

        float increaseInterval = 0.0f;
        foreach (var ent in RightSideTeam)
        {
            var posInfo = Battlefield2D.AddEntity(ent.Entity, TeamSides.RIGHT);
            
            if (posInfo == null)
                continue;

            var posTransform = Battlefield2D.GetCharacterPositionByTag(posInfo.PositionTag);
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(1);
            //sequence.Append(ent.PositionTarget.DOMove(posTransform.position + Vector3.up * 40, 0));
            sequence.Append(ent.Root.DOMove(posTransform.position + Vector3.up * 40, 0));
            sequence.AppendInterval(increaseInterval);
            increaseInterval += 0.6f;
            //sequence.Append(ent.PositionTarget.DOMove(posTransform.position, 1).SetEase(Ease.OutQuad));
            sequence.Append(ent.Root.DOMove(posTransform.position, 1).SetEase(Ease.OutQuad));
            //sequence.Append(Battlefield2D.BattlefieldCamera.DOShakePosition(0.25f, 0.25f, 30, 180));
            sequence.Append(Battlefield2D.FloorTransform.DOShakePosition(0.25f, 0.25f, 30, 180));
        }

        foreach (var ent in LeftSideTeam)
        {
            var posInfo = Battlefield2D.AddEntity(ent.Entity, TeamSides.LEFT);

            if (posInfo == null)
                continue;

            var posTransform = Battlefield2D.GetCharacterPositionByTag(posInfo.PositionTag);
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(1);
            //sequence.Append(ent.PositionTarget.DOMove(posTransform.position + Vector3.up * 40, 0));
            sequence.Append(ent.Root.DOMove(posTransform.position + Vector3.up * 40, 0));
            sequence.AppendInterval(increaseInterval);
            increaseInterval += 0.6f;
            //sequence.Append(ent.PositionTarget.DOMove(posTransform.position, 1).SetEase(Ease.OutQuad));
            sequence.Append(ent.Root.DOMove(posTransform.position, 1).SetEase(Ease.OutQuad));
            //sequence.Append(Battlefield2D.BattlefieldCamera.DOShakePosition(0.25f, 0.25f, 30, 180));
            sequence.Append(Battlefield2D.FloorTransform.DOShakePosition(0.25f, 0.25f, 30, 180));
        }

        Battlefield2D.Show();
        transform.DOMove(transform.position, 1).OnComplete(() =>
        {
            foreach (var ent in RightSideTeam.Concat(LeftSideTeam))
            {
                BattleManager2D.SetGameLayerRecursive(ent.Root.gameObject, BattleManager2D.BattlefieldLayer);
            }
        });

        transform.DOMove(transform.position, 5).OnComplete(() =>
        {
            BattleManager2D.Show();
            BattleManager2D.FillBars();
        });
    }

    internal static List<MovesIDs> CreateMoveList(int level, MovesLearnedByLevelUpData movesLearnedByLevelUp)
    {
        var moves = new List<MovesIDs>();
        for(int i = 0; i < 6; i++)
        {
            moves.Add(MovesIDs.NONE);
        }

        var possibleMoves = movesLearnedByLevelUp.MovesLearnData.Where(m => m.requiredLevel <= level).ToList();
        var chosenMoves = possibleMoves.Skip(Math.Max(0, possibleMoves.Count - 6)).ToList();

        for(int j = 0; j < chosenMoves.Count; j++)
        {
            moves[j] = chosenMoves[j].move;
        }

        return moves;
    }

    private void CorretDirection(List<AIController2D> controllers, Transform targetPos)
    {
        controllers.ForEach(c => c.UpdateFacingDirectionBasedOnPosition(targetPos.position.x));
    }
}
