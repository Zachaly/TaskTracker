import { HttpParams } from "@angular/common/http";
import PagedRequest, { mapPagedRequest } from "../PagedRequest";

export default interface GetUserTaskRequest extends PagedRequest {
    creatorId?: number,
    maxDueTimestamp?: number,
    minCreationTimestamp?: number
    listId?: number
}

export const mapGetUserTaskRequest = (request: GetUserTaskRequest): HttpParams => {
    let params = mapPagedRequest(request);

    if (request.creatorId) {
        params = params.append('CreatorId', request.creatorId)
    }
    if (request.maxDueTimestamp) {
        params = params.append('MaxDueTimestamp', request.maxDueTimestamp)
    }
    if (request.minCreationTimestamp) {
        params = params.append('MinCreationTimestamp', request.minCreationTimestamp)
    }
    if (request.listId) {
        params = params.append('ListId', request.listId)
    }

    return params;
} 