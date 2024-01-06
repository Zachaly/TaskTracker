using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.SpaceUser;
using TaskTracker.Model.SpaceUser.Request;

namespace TaskTracker.Application.Command
{
    public class GetSpaceUserQuery : GetSpaceUserRequest, IRequest<IEnumerable<SpaceUserModel>>
    {
    }

    public class GetSpaceUserHandler : IRequestHandler<GetSpaceUserQuery, IEnumerable<SpaceUserModel>>
    {
        private readonly ISpaceUserRepository _repository;

        public GetSpaceUserHandler(ISpaceUserRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<SpaceUserModel>> Handle(GetSpaceUserQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetAsync(request);
        }
    }
}
