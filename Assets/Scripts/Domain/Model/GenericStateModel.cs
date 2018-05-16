using CAFU.Core.Domain.Model;
using JetBrains.Annotations;
using UniRx;

namespace CAFU.Generics.Domain.Model
{
    [PublicAPI]
    public interface IGenericStateModel<TState> : IModel where TState : struct
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
    public class GenericStateModel<TState> : IGenericStateModel<TState> where TState : struct
    {
        private ReactiveProperty<TState> State { get; }

        public IObservable<TState> OnChangeAsObservable()
        {
            return State.AsObservable();
        }

        public IObservable<Unit> OnChangeAsObservable(TState state)
        {
            return OnChangeAsObservable().Where(x => Equals(x, state)).AsUnitObservable();
        }

        public TState GetCurrent()
        {
            return State.Value;
        }

        public void Change(TState state, bool forceNotify = false)
        {
            if (forceNotify)
            {
                State.SetValueAndForceNotify(state);
            }
            else
            {
                State.Value = state;
            }
        }

        public void Reset()
        {
            State.Value = default(TState);
        }

        public void Next()
        {
            State.Value = (dynamic) State.Value + 1;
        }

        public void Previous()
        {
            State.Value = (dynamic) State.Value - 1;
        }

        public GenericStateModel()
        {
            State = new ReactiveProperty<TState>();
        }

        public GenericStateModel(TState initialValue)
        {
            State = new ReactiveProperty<TState>(initialValue);
        }
    }
}