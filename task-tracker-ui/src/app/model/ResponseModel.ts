export default interface ResponseModel {
    error?: string,
    validationErrors?: Object,
    isSuccess: boolean
}

export interface CreatedResponseModel extends ResponseModel {
    newEntityId?: number
}