import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";

interface IProps {
	title: string;
}

const Header = (props: IProps) => {
	const { title } = props;
	return (
		<div className="mb-10 flex justify-between">
			<div className="mb-4 flex-1 text-2xl font-medium uppercase">
				{title}
			</div>
			<div className="flex">
				<div>
					<Avatar>
						<AvatarImage
							src="https://github.com/shadcn.png"
							alt="@shadcn"
						/>
						<AvatarFallback>CN</AvatarFallback>
					</Avatar>
				</div>
			</div>
		</div>
	);
};

export default Header;
