import { HttpParams } from "@angular/common/http";
import PagedRequest, { mapPagedRequest } from "../PagedRequest";

export default interface GetTaskListRequest extends PagedRequest {
    creatorId?: number,
    joinTasks?: boolean,
    joinStatusGroup?: boolean
}

export const mapGetTaskListRequest = (request: GetTaskListRequest): HttpParams => {
    let params = mapPagedRequest(request)

    if (request.creatorId) {
        params = params.append('CreatorId', request.creatorId)
    }
    if (request.joinTasks) {
        params = params.append('JoinTasks', request.joinTasks)
    }
    if (request.joinStatusGroup) {
        params = params.append('JoinStatusGroup', request.joinStatusGroup)
    }

    return params
}