import PagedRequest, { mapPagedRequest } from "../PagedRequest";

export default interface GetTaskFileAttachmentRequest extends PagedRequest {
    taskId?: number
}

export const mapGetTaskFileAttachmentRequest = (request: GetTaskFileAttachmentRequest) => {
    let params = mapPagedRequest(request)

    if(request.taskId) {
        params = params.append('TaskId', request.taskId)
    }

    return params
}