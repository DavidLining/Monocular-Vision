# -*- coding: utf-8 -*-
"""
Created on Thu May  3 11:52:57 2018

@author: Morgan.Li
"""

from math import cos,tan,sin, pi
import math
import os
import numpy as np
from numpy.linalg import inv
from transformations import quaternion_from_euler

file_path = os.path.join(os.getcwd(),  r'unity_projects\monocular_vision\Coordinates.txt')

def read_file(file_path):
    info = {"2D":None, "ScreenHW":None, "Euler":None, "3D":None}
    points_file = open(file_path, 'r', encoding='utf-8')
    points_file.seek(0, 0)
    lines = points_file.readlines();
    points_file.close()
    line_num = len(lines);
    for i in range(line_num):
        line = lines[i].strip('\n')
        if "2D" in line:
            pic_info =  lines[i+1:i+3]
            coord_l = []
            for line in pic_info:
                line = line.strip('\n')
                coord = line.split(":")[1].strip("(").strip(")").split(",")
                coord = [float(x) for x in coord]
                coord_l.append(coord)
            info["2D"] = coord_l
        if "3D" in line:
            ref_info =  lines[i+1:i+3]
            coord_l = []
            for line in ref_info:
                line = line.strip('\n')
                coord = line.split(":")[1].strip("(").strip(")").split(",")
                coord = [float(x) for x in coord]
                coord_l.append(coord)
            info["3D"] = coord_l

        if "Screen" in line:
            screen_info = line.split(":")[1].split(",")
            info["ScreenHW"] = [float(screen_info[0].strip(' ')), float(screen_info[1].strip(' '))]
        if "Camera Euler" in line:
            euler = line.split(":")[1]
            euler = euler.strip("(").strip(")").split(",")
            euler = [float(x) for x in euler]
            info["Euler"] = euler
    return info

def get_R_from_axis(angle, axis):
    """
    left hand rule
    """
    if axis == 'Z':
        R = np.array([[cos(angle), sin(angle), 0],
                     [-sin(angle), cos(angle), 0],
                     [0, 0, 1]])
    elif axis == 'Y':
        R = np.array([[cos(angle), 0, -sin(angle)],
                     [0, 1, 0],
                     [sin(angle), 0, cos(angle)]])
    elif axis == 'X':
        R = np.array([[1, 0, 0],
                    [0, cos(angle), sin(angle)],
                    [0, -sin(angle), cos(angle)]])
    return R


def rad_2_deg(rad):
    """
    transfer radian to degree
    """
    return rad/pi*180.0

def deg_2_rad(deg):
    return deg/180.0*pi

def get_scale(pt_coord, alpha, focal_len):
    beta = math.atan2(np.linalg.norm(pt_coord), focal_len)
    print("Info",(beta), sin(alpha), tan(alpha-beta))
    scale = cos(alpha) + sin(alpha)*tan(alpha-beta)
    return scale

def get_rotate_matrix(pitch, roll, yaw, angle_N2R):
    """
    get the rotate matrix
    """
    R_Xaxis = get_R_from_axis(-pitch, 'X')
    R_Yaxis = get_R_from_axis(-roll, 'Y')
    R_Zaxis = get_R_from_axis(-yaw, 'Z')
    print("Euler from Camera: \r\n", "X axis: %f, Y axis: %f, Z axis: %f\r\n"\
          %(rad_2_deg(pitch), rad_2_deg(roll), rad_2_deg(yaw)))
    print("Angle from NED CSYS to Reference CSYS: %f\r\n"%(rad_2_deg(angle_N2R)))
    R_body_2_NED = R_Zaxis.dot(R_Xaxis).dot(R_Yaxis)
    R_NED_2_REF = get_R_from_axis(angle_N2R, 'Z')

    R_body_2_REF = R_NED_2_REF.dot(R_body_2_NED)
    return R_body_2_REF

def compute_depth(euler, angle_N2R, error_angle, \
                  feature_ext_error, bands_len_decare, focal_len):

    coord_info = read_file(file_path)
    screenHW = coord_info["ScreenHW"]
    if euler == None:
        euler = coord_info["Euler"]

    pitch, roll, yaw = euler
    Params_Int = np.array([[focal_len, 0, 0], \
                           [0, focal_len, 0], \
                           [0, 0, 1]])
    Params_Int_I = inv(Params_Int)
    pitch = deg_2_rad(pitch)
    roll = deg_2_rad(roll)
    yaw = deg_2_rad(yaw)
    angle_N2R = deg_2_rad(angle_N2R)
    error_angle = deg_2_rad(error_angle)
#    q = quaternion_from_euler(pitch, roll, yaw, "szxy")
#    alpha = 2.0*(math.acos(q[0]))
   # print(rad_2_deg(alpha))

    R_body_2_REF = get_rotate_matrix(pitch, roll, yaw, angle_N2R)
    R_body_2_REF_with_error = get_rotate_matrix(pitch+error_angle, \
                                                roll+ error_angle,\
                                                yaw+ error_angle, \
                                                angle_N2R+ error_angle)

    center = [x/2 for x in screenHW]+[0.0]
    u0_v0 = np.array(center)

    a = np.array(coord_info["2D"][0])
    b = np.array(coord_info["2D"][1])

    a = a - u0_v0
    b = b - u0_v0
    print("Img pts coordinate:", "\r\nA:", a, "\r\nB:", b)

    R = Params_Int.dot(R_body_2_REF).dot(Params_Int_I)
   # R = R_body_2_REF

    a_r = (R).dot(a)
    b_r = (R).dot(b)
    a_r /= a_r[2]
    b_r /= b_r[2]

    b_r[0] -= feature_ext_error
    print("Extract Points coordinate:", "\r\nA:", a_r, "\r\nB:", b_r)

    len_ab_r = np.linalg.norm(a_r-b_r)
    depth = bands_len_decare*focal_len/len_ab_r
    print("\r\nDepth:", depth)

def calibrate(bands_len_decare, depth):
    coord_info = read_file(file_path)
    screenHW = coord_info["ScreenHW"]
    center = [x/2 for x in screenHW]+[0.0]
    u0_v0 = np.array(center)

    a = np.array(coord_info["2D"][0])
    b = np.array(coord_info["2D"][1])

    a = a - u0_v0
    b = b - u0_v0
    len_ab = np.linalg.norm(a-b)
    focal_len = depth*len_ab/bands_len_decare
    print("Focal:", focal_len)
    return focal_len
#12个
depth_true = 100
focal_len = 440.191
bands_len_decare = 40
error_deg = 0.5
angle_N2R = 0
feature_ext_error = 0 # feature extraction error

#focal_len = calibrate(bands_len_decare, depth_true)

compute_depth(None, angle_N2R, error_deg, \
              feature_ext_error, bands_len_decare, focal_len)


