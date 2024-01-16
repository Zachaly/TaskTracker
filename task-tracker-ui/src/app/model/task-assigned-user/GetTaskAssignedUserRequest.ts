import PagedRequest, { mapPagedRequest } from "../PagedRequest";

export default interface GetTaskAssignedUserRequest extends PagedRequest {
    taskId?: number,
    userId?: number
}

export const mapGetTaskAssignedUserRequest = (request: GetTaskAssignedUserRequest) => {
    let params = mapPagedRequest(request)

    if(request.taskId) {
        params = params.append('TaskId', request.taskId)
    }

    if(request.userId) {
        params = params.append('UserId', request.userId)
    }

    return params
}