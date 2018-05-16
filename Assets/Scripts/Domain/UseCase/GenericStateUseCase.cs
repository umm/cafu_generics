using CAFU.Core.Domain.UseCase;
using CAFU.Generics.Domain.Model;
using JetBrains.Annotations;
using UniRx;

namespace CAFU.Generics.Domain.UseCase
{
    [PublicAPI]
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

    [PublicAPI]
    public interface ISingletonGenericStateUseCase<TState> : IGenericStateUseCase<TState>, ISingletonUseCase where TState : struct
    {
    }

    [PublicAPI]
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
            return GenericStateModel.OnChangeAsObservable();
        }

        public IObservable<Unit> OnChangeAsObservable(TState state)
        {
            return GenericStateModel.OnChangeAsObservable(state);
        }

        public TState GetCurrent()
        {
            return GenericStateModel.GetCurrent();
        }

        public void Change(TState state, bool forceNotify = false)
        {
            GenericStateModel.Change(state, forceNotify);
        }

        public void Reset()
        {
            GenericStateModel.Reset();
        }

        public void Next()
        {
            GenericStateModel.Next();
        }

        public void Previous()
        {
            GenericStateModel.Previous();
        }
    }

    [PublicAPI]
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