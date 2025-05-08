image:
	docker build . -t tracking-solution

run:
	sh -c '(cd ../run && \
	 docker run -v $(pwd):/data tracking-solution --input /data/input_data.json --output /data/tracking_output.json --vis-dir /data/visualization)'

