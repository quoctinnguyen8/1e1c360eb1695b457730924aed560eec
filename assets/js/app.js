Vue.createApp({
	data() {
		return {
			mssv: '',
			maLop: '',
			listData: null,
			errMesg: null
		}
	},
	methods: {
		search() {
			if (!this.mssv && !this.maLop) {
				this.errMesg = "Phải nhập mã sinh viên hoặc mã lớp";
				return;
			} else {
				this.errMesg = null;
			}

			axios.get('/api/search', {
				params: {
					MaSinhVien: this.mssv,
					LopHoc: this.maLop
				}
			})
			.then(json => {
				this.listData = json.data;
			});
		}
	}
})
.mount('.container');