using Examination.Domain.AggregateModels.UserAggregate;
using Examination.Domain.Events;
using MediatR;

namespace Examination.Application.DomainEventHandlers.V1
{
    public class SynchronizeUserWhenExamStartedDomainEventHandler : INotificationHandler<ExamStartedDomainEvent>
    {
        private readonly IUserRepository _userRepository;
        public SynchronizeUserWhenExamStartedDomainEventHandler(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(ExamStartedDomainEvent notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(notification.UserId);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            if (user == null)
            {
                _userRepository.StartTransaction();
                user = User.CreateNewUser(notification.UserId, notification.FirstName, notification.LastName);
                await _userRepository.InsertAsync(user);
                await _userRepository.CommitTransactionAsync(user, cancellationToken);
            }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
    }
}