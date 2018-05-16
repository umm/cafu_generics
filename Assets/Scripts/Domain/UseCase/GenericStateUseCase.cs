﻿using CAFU.Core.Domain.UseCase;
using CAFU.Generics.Domain.Model;
using UniRx;

namespace CAFU.Generics.Domain.UseCase
{
    public interface IGenericStateUseCase<TState> : IUseCase where TState : struct
    {
        IObservable<TState> OnChangeAsObservable();

        IObservable<Unit> OnChangeAsObservable(TState state);

        TState GetCurrent();

        void Change(TState state, bool forceNotify = false);

        void Reset();

        void Next();

        void Previous();
    }

    public interface ISingletonGenericStateUseCase<TState> : IGenericStateUseCase<TState>, ISingletonUseCase where TState : struct
    {
    }

    public class GenericStateUseCase<TState> : IGenericStateUseCase<TState> where TState : struct
    {
        public class Factory : DefaultUseCaseFactory<GenericStateUseCase<TState>>
        {
            protected override void Initialize(GenericStateUseCase<TState> instance)
            {
                base.Initialize(instance);
                instance.GenericStateModel = new GenericStateModel<TState>();
            }
        }

        protected IGenericStateModel<TState> GenericStateModel { private get; set; }

        public IObservable<TState> OnChangeAsObservable()
        {
            return this.GenericStateModel.OnChangeAsObservable();
        }

        public IObservable<Unit> OnChangeAsObservable(TState state)
        {
            return this.GenericStateModel.OnChangeAsObservable(state);
        }

        public TState GetCurrent()
        {
            return this.GenericStateModel.GetCurrent();
        }

        public void Change(TState state, bool forceNotify = false)
        {
            this.GenericStateModel.Change(state, forceNotify);
        }

        public void Reset()
        {
            this.GenericStateModel.Reset();
        }

        public void Next()
        {
            this.GenericStateModel.Next();
        }

        public void Previous()
        {
            this.GenericStateModel.Previous();
        }
    }

    public class SingletonGenericStateUseCase<TState> : GenericStateUseCase<TState>, ISingletonGenericStateUseCase<TState> where TState : struct
    {
        public new class Factory : DefaultUseCaseFactory<SingletonGenericStateUseCase<TState>>
        {
            protected override void Initialize(SingletonGenericStateUseCase<TState> instance)
            {
                base.Initialize(instance);
                instance.GenericStateModel = new GenericStateModel<TState>();
            }
        }
    }
}