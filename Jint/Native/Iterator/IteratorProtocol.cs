﻿using Jint.Native.Array;
using Jint.Native.Object;
using Jint.Runtime;

namespace Jint.Native.Iterator
{
    /// <summary>
    /// Handles looping of iterator values, sub-classes can use to implement wanted actions.
    /// </summary>
    internal abstract class IteratorProtocol
    {
        protected readonly Engine _engine;
        private readonly IIterator _iterator;
        private readonly int _argCount;

        protected IteratorProtocol(
            Engine engine,
            IIterator iterator,
            int argCount)
        {
            _engine = engine;
            _iterator = iterator;
            _argCount = argCount;
        }

        internal void Execute()
        {
            var args = _engine._jsValueArrayPool.RentArray(_argCount);
            try
            {
                do
                {
                    var item = _iterator.Next();
                    if (item.TryGetValue(CommonProperties.Done, out var done) && done.AsBoolean())
                    {
                        break;
                    }

                    if (!item.TryGetValue(CommonProperties.Value, out var currentValue))
                    {
                        currentValue = JsValue.Undefined;
                    }

                    ProcessItem(args, currentValue);
                } while (ShouldContinue);
            }
            catch
            {
                ReturnIterator();
                throw;
            }
            finally
            {
                _engine._jsValueArrayPool.ReturnArray(args);
            }

            IterationEnd();
        }

        protected void ReturnIterator()
        {
            _iterator.Return();
        }

        protected virtual bool ShouldContinue => true;

        protected virtual void IterationEnd()
        {
        }

        protected abstract void ProcessItem(JsValue[] args, JsValue currentValue);

        protected static JsValue ExtractValueFromIteratorInstance(JsValue jsValue)
        {
            if (jsValue is ArrayInstance ai)
            {
                uint index = 0;
                if (ai.GetLength() > 1)
                {
                    index = 1;
                }

                ai.TryGetValue(index, out var value);
                return value;
            }

            return jsValue;
        }

        internal static void AddEntriesFromIterable(ObjectInstance target, IIterator iterable, object adder)
        {
            if (!(adder is ICallable callable))
            {
                ExceptionHelper.ThrowTypeError(target.Engine, "set must be callable");
                return;
            }

            var close = false;
            var args = target.Engine._jsValueArrayPool.RentArray(2);
            try
            {
                do
                {
                    var item = iterable.Next();
                    if (item.TryGetValue(CommonProperties.Done, out var done) && done.AsBoolean())
                    {
                        close = true;
                        break;
                    }

                    if (!item.TryGetValue(CommonProperties.Value, out var currentValue))
                    {
                        currentValue = JsValue.Undefined;
                    }

                    close = true;
                    if (!(currentValue is ObjectInstance oi))
                    {
                        ExceptionHelper.ThrowTypeError(target.Engine, "iterator's value must be an object");
                        return;
                    }

                    var k = oi.Get(JsString.NumberZeroString);
                    var v = oi.Get(JsString.NumberOneString);

                    args[0] = k;
                    args[1] = v;

                    callable.Call(target, args);
                } while (true);
            }
            catch
            {
                if (close)
                {
                    iterable.Return();
                }
                throw;
            }
            finally
            {
                target.Engine._jsValueArrayPool.ReturnArray(args);
            }
        }

    }
}