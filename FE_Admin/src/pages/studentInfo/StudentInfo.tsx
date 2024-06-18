import { StudentFilterDetail } from "@/apis/student.api";
import { Separator } from "@/components/ui/separator";
import { IAppState, useAppSelector } from "@/redux/store";
import { IStudent } from "@/types/student.type";
import { useEffect, useState } from "react";

const StudentInfo = () => {
	const { accoundId } = useAppSelector((state: IAppState) => state.users);
	const [info, setInfo] = useState<IStudent>();

	useEffect(() => {
		StudentFilterDetail(accoundId!).then((res) => {
			setInfo(res?.data.data);
		});
	}, []);
	return (
		<>
			<div className="mb-4 text-2xl font-semibold uppercase">
				Hồ sơ học sinh
			</div>
			<div className="mt-5 p-5">
				<div className="text-xl font-semibold text-primary">
					Thông tin học sinh
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
									<div className="font-medium">Giới tính</div>
									<div>{info?.gender!}</div>
								</div>
							</div>
							<div className="mb-4 flex items-center ">
								<div className="flex w-[400px] gap-3">
									<div className="font-medium">Email:</div>
									<div>{info?.email}</div>
								</div>
								<div className="flex gap-3">
									<div className="font-medium">
										Niên khóa:
									</div>
									<div>{info?.academicYear}</div>
								</div>
							</div>
							<div className="mb-4 flex gap-3">
								<div className="font-medium">
									Tình trạng học tập:{" "}
								</div>
								<div>{info?.status}</div>
							</div>
						</div>
						<div>
							<img
								src={
									info?.avatar ||
									"https://o.remove.bg/downloads/46adaa72-dcc7-45bc-8136-8bf5f658b5cb/123-removebg-preview.png"
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
									<div>{info?.dob}</div>
								</div>
								<div className="flex gap-3 ">
									<div className="font-medium">Dân tộc:</div>
									<div>{info?.ethnic}</div>
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
									<div className="font-medium">Email:</div>
									<div>{info?.email}</div>
								</div>
							</div>
							<div className="mb-4 flex items-center ">
								<div className="flex w-[400px] gap-3">
									<div className="font-medium">CCCD:</div>
									<div>{info?.identificationNumber}</div>
								</div>
								<div className="flex gap-3">
									<div className="font-medium">Địa chỉ:</div>
									<div>{info?.address}</div>
								</div>
							</div>

							<div className="mt-10">
								<div className="mb-4 flex items-center ">
									<div className="flex w-[400px] gap-3">
										<div className="font-medium">
											Họ và tên bố:
										</div>
										<div>{info?.fatherName}</div>
									</div>
									<div className="flex gap-3 ">
										<div className="font-medium">
											Họ và tên mẹ:
										</div>
										<div>{info?.motherName}</div>
									</div>
								</div>

								<div className="mb-4 flex items-center ">
									<div className="flex w-[400px] gap-3">
										<div className="font-medium">
											Nghề nghiệp bố:
										</div>
										<div>{info?.fatherJob}</div>
									</div>
									<div className="flex gap-3 ">
										<div className="font-medium">
											Nghề nghiệp mẹ:
										</div>
										<div>{info?.motherJob}</div>
									</div>
								</div>

								<div className="mb-4 flex items-center ">
									<div className="flex w-[400px] gap-3">
										<div className="font-medium">
											Số điện thoại bố:
										</div>
										<div>{info?.fatherPhoneNumber}</div>
									</div>
									<div className="flex gap-3 ">
										<div className="font-medium">
											Số điện thoại mẹ:
										</div>
										<div>{info?.motherPhoneNumber}</div>
									</div>
								</div>
								<div className="mb-4 flex items-center ">
									<div className="flex w-[400px] gap-3">
										<div className="font-medium">
											Email bố:
										</div>
										<div>{info?.fatherEmail}</div>
									</div>
									<div className="flex gap-3 ">
										<div className="font-medium">
											Email mẹ:
										</div>
										<div>{info?.motherEmail}</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</>
	);
};

export default StudentInfo;
