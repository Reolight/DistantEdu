export const TEACHER_ROLE = `teacher`
export const ADMIN_ROLE = `admin`
export const STUDENT_ROLE = `student`

export function authenticate(userRole, minRole){
    switch (minRole){
        case STUDENT_ROLE:
            return true;
        case TEACHER_ROLE:
            return userRole !== STUDENT_ROLE
        case ADMIN_ROLE:
            return userRole === ADMIN_ROLE
    }
}