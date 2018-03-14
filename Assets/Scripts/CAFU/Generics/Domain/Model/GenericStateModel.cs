using CAFU.Core.Domain.Model;
using UniRx;

namespace CAFU.Generics.Domain.Model {

    public interface IGenericStateModel<TState> : IModel where TState : struct {

        IObservable<TState> OnChangeAsObservable();

        IObservable<Unit> OnChangeAsObservable(TState state);

        TState GetCurrent();

        void Change(TState state, bool forceNotify = false);

        void Reset();

        void Next();

        void Previous();

    }

    public class GenericStateModel<TState> : IGenericStateModel<TState> where TState : struct {

        private ReactiveProperty<TState> State { get; }

        public IObservable<TState> OnChangeAsObservable() {
            return this.State.AsObservable();
        }

        public IObservable<Unit> OnChangeAsObservable(TState state) {
            return this.OnChangeAsObservable().Where(x => Equals(x, state)).AsUnitObservable();
        }

        public TState GetCurrent() {
            return this.State.Value;
        }

        public void Change(TState state, bool forceNotify = false) {
            if (forceNotify) {
                this.State.SetValueAndForceNotify(state);
            } else {
                this.State.Value = state;
            }
        }

        public void Reset() {
            this.State.Value = default(TState);
        }

        public void Next() {
            this.State.Value = (dynamic)this.State.Value + 1;
        }

        public void Previous() {
            this.State.Value = (dynamic)this.State.Value - 1;
        }

        public GenericStateModel() {
            this.State = new ReactiveProperty<TState>();
        }

        public GenericStateModel(TState initialValue) {
            this.State = new ReactiveProperty<TState>(initialValue);
        }

    }

}