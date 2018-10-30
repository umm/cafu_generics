using System;
using System.Collections.Generic;
using System.Linq;
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
            var type = typeof(TState);
            if (type.IsEnum)
            {
                var currentIndex = enumValueList.IndexOf(State.Value);
                if (enumValueList.Count > currentIndex + 1)
                {
                    State.Value = enumValueList[currentIndex + 1];
                }
            }
            else
            {
                throw new InvalidOperationException("Cannot call Next() to non-enum typed GenericStateModel. Because iOS is not allow `dynamic' call.");
            }
        }

        public void Previous()
        {
            var type = typeof(TState);
            if (type.IsEnum)
            {
                var currentIndex = enumValueList.IndexOf(State.Value);
                if (currentIndex > 0)
                {
                    State.Value = enumValueList[currentIndex - 1];
                }
            }
            else
            {
                throw new InvalidOperationException("Cannot call Previous() to non-enum typed GenericStateModel. Because iOS is not allow `dynamic' call.");
            }
        }

        public GenericStateModel()
        {
            State = new ReactiveProperty<TState>();
            Initialize();
        }

        public GenericStateModel(TState initialValue)
        {
            State = new ReactiveProperty<TState>(initialValue);
            Initialize();
        }

        private IList<TState> enumValueList;

        private void Initialize()
        {
            var type = typeof(TState);
            if (type.IsEnum)
            {
                enumValueList = new List<TState>(Enum.GetValues(type).OfType<TState>());
            }
        }
    }
}