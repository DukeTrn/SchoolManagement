import { Separator } from "@/components/ui/separator";
import { getTeacherFilterDetail } from "@/apis/teacher.api";
import { IAppState, useAppSelector } from "@/redux/store";
import { useEffect, useState } from "react";

import { ITeacher } from "@/types/teacher.type";

const TeacherInfo = () => {
	const { accoundId } = useAppSelector((state: IAppState) => state.users);
	const [info, setInfo] = useState<ITeacher>();

	useEffect(() => {
		getTeacherFilterDetail(accoundId!).then((res) => {
			setInfo(res?.data.data);
		});
	}, []);
	return (
		<>
			<div className="mb-4 text-2xl font-semibold uppercase">
				Hồ sơ giáo viên
			</div>
			<div className="mt-5 p-5">
				<div className="text-xl font-semibold text-primary">
					Thông tin giảng dạy
				</div>
				<div className="mt-4">
					<div className="flex gap-10">
						<div className="flex-1 flex-col">
							<div className="mb-4 flex items-center ">
								<div className="flex w-[400px] gap-3">
									<div className="font-medium">
										Họ và tên:
									</div>
									<div>{info?.fullName}</div>
								</div>
								<div className="flex gap-3 ">
									<div className="font-medium">
										Tổ bộ môn:
									</div>
									<div>{info?.departmentName!}</div>
								</div>
							</div>
							<div className="mb-4 flex items-center ">
								<div className="flex w-[400px] gap-3">
									<div className="font-medium">Email:</div>
									<div>{info?.email}</div>
								</div>
								<div className="flex gap-3">
									<div className="font-medium">Trình độ:</div>
									<div>{info?.level}</div>
								</div>
							</div>
							<div className="mb-4 flex gap-3">
								<div className="font-medium">
									Thời gian bắt đầu giảng dạy:
								</div>
								<div>{info?.timeStart}</div>
							</div>
							<div className="mb-4 flex gap-3">
								<div className="font-medium">
									Tình trạng giảng dạy:
								</div>
								<div>{info?.status}</div>
							</div>
						</div>
						<div>
							<img
								src={
									info?.avatar ||
									"https://github.com/shadcn.png"
								}
								alt=""
								className="h-[160px] w-[160px]"
							/>
						</div>
					</div>
				</div>
				<Separator className="mt-8 h-[2px] bg-primary font-semibold" />
				<div className="mt-4 text-xl font-semibold text-primary">
					Thông tin cá nhân
				</div>
				<div className="mt-4">
					<div className="flex gap-10">
						<div className="flex-1 flex-col">
							<div className="mb-4 flex items-center ">
								<div className="flex w-[400px] gap-3">
									<div className="font-medium">
										Ngày sinh:
									</div>
									<div>xxx</div>
								</div>
								<div className="flex gap-3 ">
									<div className="font-medium">
										Giới tính:
									</div>
									<div>xxx</div>
								</div>
							</div>
							<div className="mb-4 flex items-center ">
								<div className="flex w-[400px] gap-3">
									<div className="font-medium">
										Số điện thoại:
									</div>
									<div>{info?.phoneNumber}</div>
								</div>
								<div className="flex gap-3">
									<div className="font-medium">Dân tộc:</div>
									<div>{info?.ethnic}</div>
								</div>
							</div>
							<div className="mb-4 flex items-center ">
								<div className="flex w-[400px] gap-3">
									<div className="font-medium">CCCD:</div>
									<div>{info?.identificationNumber}</div>
								</div>
								<div className="flex gap-3">
									<div className="font-medium">Email:</div>
									<div>{info?.email}</div>
								</div>
							</div>

							<div className="mb-4 flex gap-3">
								<div className="font-medium">Địa chỉ:</div>
								<div>{info?.address}</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</>
	);
};

export default TeacherInfo;
