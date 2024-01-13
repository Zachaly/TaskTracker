import UserSpaceModel from "../user-space/UserSpaceModel";
import UserModel from "../user/UserModel";

export default interface SpaceUserModel {
    user: UserModel,
    space: UserSpaceModel
}