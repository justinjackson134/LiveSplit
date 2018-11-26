﻿using LiveSplit.Model.Input;
using System;
using System.IO;

namespace LiveSplit.Model
{
    public interface ITimerModel
    {
        LiveSplitState CurrentState { get; set; }

        event EventHandler OnSplit;
        event EventHandler OnUndoSplit;
        event EventHandler OnSkipSplit;
        event EventHandler OnStart;
        event EventHandler OnResumePreviousRun;
        event EventHandlerT<TimerPhase> OnReset;
        event EventHandler OnPause;
        event EventHandler OnUndoAllPauses;
        event EventHandler OnResume;
        event EventHandler OnScrollUp;
        event EventHandler OnScrollDown;
        event EventHandler OnSwitchComparisonPrevious;
        event EventHandler OnSwitchComparisonNext;
        
        void Start();
        void ResumePreviousRun(Stream stream);
        void InitializeGameTime();
        void Split();
        void SkipSplit();
        void UndoSplit();
        void Reset();
        void Reset(bool updateSplits);
        void ResetAndSetAttemptAsPB();
        void Pause();
        void UndoAllPauses();
        void ScrollUp();
        void ScrollDown();
        void SwitchComparisonPrevious();
        void SwitchComparisonNext();
    }
}
