const path = {
	//admin
	login: "/login",
	home: "/home",
	account: "/account",
	student: "/student",
	department: "/department",
	departmentDetails: "/department/:id",
	teacher: "/teacher",
	semester: "/semester",
	semesterDetail: "/semester/:id",
	classroom: "/classroom",
	classroomDetail: "/classroom/:id",
	study: "/study",
	studyClass: "/study/:id",
	studyStudent: "/study/:id/:id1",
	assignment: "/assignment",
	assignmentDetail: "assignment/:id",
	conduct: "/conduct",
	conductClass: "/conduct/:id",

	//teacher
	teacherInfo: "/teacher-info",
	teacherDepartment: "/teacher-department",
	teacherClass: "/teacher-class",
	teacherClassDetail: "/teacher-class/:id",
	teacherStudentDetail: "/teacher-class/:id/:id1",
	teacherHomeroom: "/teacher-homeroom",

	//student
	studentInfo: "/student-info",
	studentClass: "/student-class",
	studentClassDetail: "/student-class/:id",
	studentClassDetailInfo: "/student-class/:id/:id1",
} as const;

export default path;
