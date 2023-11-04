using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class UpdateTaskListCommandValidator : AbstractValidator<UpdateTaskListCommand>
    {
        public UpdateTaskListCommandValidator()
        {
            
        }
    }
}
