import path from "@/constants/path";
import MainLayout from "@/layouts/MainLayout";
import Home from "@/pages/home";
import { Navigate, Outlet, useRoutes } from "react-router-dom";
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
import AssignmentDetails from "@/pages/assignment/detail/AssignmentDetail";
import { IAppState, useAppSelector } from "@/redux/store";
import Login from "@/pages/login";
import { userRole } from "@/utils/utils";
import Study from "@/pages/study";
import StudyClassDetail from "@/pages/study/ClassDetail";
import StudyStudentDetail from "@/pages/study/StudentDetail";
import Conduct from "@/pages/conduct";
import ConductClass from "@/pages/conduct/detail";

const ProtectedRoute = () => {
	const { accoundId, role } = useAppSelector(
		(state: IAppState) => state.users
	);
	return accoundId ? <Outlet /> : <Navigate to={path.login} />;
};
const RejectedRoute = () => {
	const { accoundId, role } = useAppSelector(
		(state: IAppState) => state.users
	);
	return !accoundId ? <Outlet /> : <Navigate to={path.home} />;
};

const useRouteElement = () => {
	const { accoundId, role } = useAppSelector(
		(state: IAppState) => state.users
	);
	const isAdmin = role === userRole.admin;

	const routeElement = useRoutes([
		{
			path: "",
			element: <ProtectedRoute />,
			children: [
				{
					path: "/",
					element: <Navigate to={path.home} replace />,
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
					element: isAdmin && (
						<MainLayout>
							<Account />
						</MainLayout>
					),
				},
				{
					path: path.student,
					element: isAdmin && (
						<MainLayout>
							<Student />
						</MainLayout>
					),
				},
				{
					path: path.department,
					element: isAdmin && (
						<MainLayout>
							<Department />
						</MainLayout>
					),
				},
				{
					path: path.departmentDetails,
					element: isAdmin && (
						<MainLayout>
							<DepartmentDetail />
						</MainLayout>
					),
				},
				{
					path: path.teacher,
					element: isAdmin && (
						<MainLayout>
							<Teacher />
						</MainLayout>
					),
				},
				{
					path: path.semester,
					element: isAdmin && (
						<MainLayout>
							<Semester />
						</MainLayout>
					),
				},
				{
					path: path.semesterDetail,
					element: isAdmin && (
						<MainLayout>
							<SemesterDetail />
						</MainLayout>
					),
				},
				{
					path: path.classroom,
					element: isAdmin && (
						<MainLayout>
							<Classroom />
						</MainLayout>
					),
				},
				{
					path: path.classroomDetail,
					element: isAdmin && (
						<MainLayout>
							<ClassroomDetail />
						</MainLayout>
					),
				},
				{
					path: path.assignment,
					element: isAdmin && (
						<MainLayout>
							<Assignment />
						</MainLayout>
					),
				},
				{
					path: path.assignmentDetail,
					element: isAdmin && (
						<MainLayout>
							<AssignmentDetails />
						</MainLayout>
					),
				},
				{
					path: path.study,
					element: isAdmin && (
						<MainLayout>
							<Study />
						</MainLayout>
					),
				},
				{
					path: path.studyClass,
					element: isAdmin && (
						<MainLayout>
							<StudyClassDetail />
						</MainLayout>
					),
				},
				{
					path: path.studyStudent,
					element: isAdmin && (
						<MainLayout>
							<StudyStudentDetail />
						</MainLayout>
					),
				},
				{
					path: path.conduct,
					element: isAdmin && (
						<MainLayout>
							<Conduct />
						</MainLayout>
					),
				},
				{
					path: path.conductClass,
					element: isAdmin && (
						<MainLayout>
							<ConductClass />
						</MainLayout>
					),
				},
			],
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
			path: "",
			element: <RejectedRoute />,
			children: [
				{
					path: path.login,
					element: <Login />,
				},
			],
		},
	]);
	return routeElement;
};

export default useRouteElement;
