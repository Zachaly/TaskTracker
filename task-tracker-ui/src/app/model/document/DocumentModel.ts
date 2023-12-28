import DocumentPageModel from "../document-page/DocumentPageModel";
import UserModel from "../user/UserModel";

export default interface DocumentModel {
    id: number,
    creator: UserModel,
    title: string,
    creationTimestamp: number,
    pages: DocumentPageModel[]
}