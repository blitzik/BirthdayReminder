﻿using Caliburn.Micro;
using Common.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Common.FlashMessages
{
    public class FlashMessageDecorator : PropertyChangedBase
    {
        private IFlashMessage _flashMessage;
        public IFlashMessage FlashMessage
        {
            get { return _flashMessage; }
        }


        private long _disposeAt;
        public long DisposeAt
        {
            get { return _disposeAt; }
        }


        public Type Type
        {
            get { return FlashMessage.Type; }
        }


        public bool CanBeDisposed
        {
            get { return FlashMessage.CanBeDisposed; }
        }


        private DelegateCommand<object> _disposeCommand;
        public DelegateCommand<object> DisposeCommand
        {
            get
            {
                if (_disposeCommand == null) {
                    _disposeCommand = new DelegateCommand<object>(p => Dispose());
                }
                return _disposeCommand;
            }
        }


        public FlashMessageDecorator(IFlashMessage flashMessage)
        {
            _flashMessage = flashMessage;
            _disposeAt = DateTimeOffset.UtcNow.Add(flashMessage.Lifespan).ToUnixTimeMilliseconds();
        }


        public void MarkFlashMessageAsDisposable()
        {
            FlashMessage.MarkAsDisposable();
            NotifyOfPropertyChange(() => CanBeDisposed);
        }


        public bool CanBeRemoved(long now)
        {
            return (_disposeAt + 2000) <= now;
        }


        private void Dispose()
        {
            MarkFlashMessageAsDisposable();
        }
    }
}
