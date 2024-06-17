import path from "@/constants/path";
import MainLayout from "@/layouts/MainLayout";
import Home from "@/pages/home";
import { Navigate, useRoutes } from "react-router-dom";
import Account from "@/pages/account";
import Student from "@/pages/student";
import Department from "@/pages/department";
import DepartmentDetail from "@/pages/department/details/DepartmentDetail";
import Teacher from "@/pages/teacher/Teacher";
import Semester from "@/pages/semester";
import SemesterDetail from "@/pages/semester/details/SemesterDetails";
import Classroom from "@/pages/classroom";
import NotFound from "@/pages/notfound";
import ClassroomDetail from "@/pages/classroom/details/ClassDetails";
import Assignment from "@/pages/assignment";

const useRouteElement = () => {
	const routeElement = useRoutes([
		{
			path: "/",
			element: <Navigate to={path.home} replace />,
		},
		{
			path: "*",
			element: (
				<MainLayout>
					<NotFound />
				</MainLayout>
			),
		},
		{
			index: true,
			path: path.home,
			element: (
				<MainLayout>
					<Home />
				</MainLayout>
			),
		},
		{
			path: path.account,
			element: (
				<MainLayout>
					<Account />
				</MainLayout>
			),
		},
		{
			path: path.student,
			element: (
				<MainLayout>
					<Student />
				</MainLayout>
			),
		},
		{
			path: path.department,
			element: (
				<MainLayout>
					<Department />
				</MainLayout>
			),
		},
		{
			path: path.departmentDetails,
			element: (
				<MainLayout>
					<DepartmentDetail />
				</MainLayout>
			),
		},
		{
			path: path.teacher,
			element: (
				<MainLayout>
					<Teacher />
				</MainLayout>
			),
		},
		{
			path: path.semester,
			element: (
				<MainLayout>
					<Semester />
				</MainLayout>
			),
		},
		{
			path: path.semesterDetail,
			element: (
				<MainLayout>
					<SemesterDetail />
				</MainLayout>
			),
		},
		{
			path: path.classroom,
			element: (
				<MainLayout>
					<Classroom />
				</MainLayout>
			),
		},
		{
			path: path.classroomDetail,
			element: (
				<MainLayout>
					<ClassroomDetail />
				</MainLayout>
			),
		},
		{
			path: path.assignment,
			element: (
				<MainLayout>
					<Assignment />
				</MainLayout>
			),
		},
	]);
	return routeElement;
};

export default useRouteElement;
