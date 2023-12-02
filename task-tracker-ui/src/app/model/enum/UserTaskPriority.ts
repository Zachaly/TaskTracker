enum UserTaskPriority {
    low = 0,
    medium = 1,
    high = 2,
    urgent = 3
}



export default UserTaskPriority

export const priorityColor = (priority?: UserTaskPriority): string => {
    if (priority === undefined || priority === null) {
        return '#fafafa'
    }

    if (priority == UserTaskPriority.urgent) {
        return '#e02626'
    }
    else if (priority == UserTaskPriority.medium) {
        return '#0bcde3'
    }
    else if (priority == UserTaskPriority.high) {
        return '#e7ed32'
    }
    else if (priority == UserTaskPriority.low) {
        return '#7f8485'
    }

    return '#ad0505'
}