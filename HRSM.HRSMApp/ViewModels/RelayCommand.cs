using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels
{
    /// <summary>
    /// 命令实现类
    /// </summary>
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        /// <summary>
        /// 要执行的操作
        /// </summary>
        private Action<object> executeActions;
        /// <summary>
        /// 是否可以执行的委托
        /// </summary>
        private Func<object, bool> canExecuteFunc;
        /// <summary>
        /// 构造函数 无参构造
        /// </summary>
        public RelayCommand() { }
        /// <summary>
        /// 通过执行的委托构造
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<object> execute) : this(execute, null)
        {

        }
        /// <summary>
        /// 通过执行的操作与是否可执行的委托
        /// </summary>
        /// <param name="execute">要执行的操作</param>
        /// <param name="canExecute">是否可以被执行</param>
        public RelayCommand(Action<object> execute,Func<object,bool> canExecute)
        {
            this.executeActions = execute;
            this.canExecuteFunc = canExecute;
        }

        /// <summary>
        /// 命令是否可以执行
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            if (canExecuteFunc != null)
                return this.canExecuteFunc(parameter);
            else
                return true;
        }

        /// <summary>
        /// 要执行的操作
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            if (executeActions == null)
                return;
            this.executeActions(parameter);
        }

        /// <summary>
        /// 执行CanExecuteChanged事件
        /// </summary>
        public void OnCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
