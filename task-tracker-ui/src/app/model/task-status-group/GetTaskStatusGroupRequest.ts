import PagedRequest, { mapPagedRequest } from "../PagedRequest";

export interface GetTaskStatusGroupRequest extends PagedRequest {
    userId?: number
}

export const mapGetTaskStatusGroupRequest = (request: GetTaskStatusGroupRequest) => {
    let params = mapPagedRequest(request)

    if (request.userId) {
        params = params.append('UserId', request.userId)
    }

    return params
}